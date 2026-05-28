using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Configs;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using UniRx;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Vision
{
    public class VisionMap : MonoBehaviour
    {
        [SerializeField] private FogOfWar _fogOfWar;
        [SerializeField] private VisionConfig _config;

        private bool _isRecalculating;

        public HashSet<VisionSource> VisionSources { get; } = new();
        public HashSet<VisionSource> SimulatedVisionSources { get; } = new();
        public HashSet<VisionSource> IdleVisionSources { get; } = new();
        public HashSet<VisionSource> PlayerVisionSources { get; } = new();
        public HashSet<VisionSource> EnemyVisionSources { get; } = new();
        public HashSet<Bounds> PlayerBounds { get; } = new();
        public HashSet<Bounds> PlayerSimulationBounds { get; } = new();

        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private IsometricOverlap IsometricOverlap { get; set; }

        private void Awake()
        {
            this.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.RecalculationPeriod))
                .Where(_ => ! _isRecalculating)
                .Subscribe(_ => Recalculate());
        }

        public HashSet<Unit> GetUnitsVisibleBy(Owner owner)
        {
            return VisionSources
                .Where(v => v.Owner == owner)
                .SelectMany(v => v.VisibleUnits)
                .ToHashSet();
        }
        
        public bool IsPointVisibleBy(Vector2 point, Owner owner)
        {
            foreach (VisionSource visionSource in SimulatedVisionSources)
            {
                if (visionSource.Owner != owner)
                    continue;
                if (visionSource.IsPointVisible(point))
                    return true;
            }
            return false;
        }

        public bool IsPointSimulated(Vector2 point)
        {
            foreach (Bounds bounds in PlayerSimulationBounds)
            {
                if (point.x >= bounds.min.x && point.x <= bounds.max.x
                    && point.y >= bounds.min.y && point.y <= bounds.max.y)
                    return true;
            }
            return false;
        }

        public bool IsUnitVisibleBy(Unit unit, Owner owner) => GetUnitsVisibleBy(owner).Contains(unit);

        public VisionSource CreateSource(Func<Vector3> position, Func<Owner> owner, Func<float> radius, Func<bool> isAir)
        {
            VisionSource source = new(this, _config, IsometricOverlap, position, owner, radius, isAir);
            VisionSources.Add(source);
            return source;
        }

        private async UniTask Recalculate()
        {
            _isRecalculating = true;
            
            SimulatedVisionSources.Clear();
            IdleVisionSources.Clear();
            PlayerVisionSources.Clear();
            PlayerBounds.Clear();
            PlayerSimulationBounds.Clear();
            EnemyVisionSources.Clear();

            VisionSources.RemoveWhere(v => v.Disposed);
            
            foreach (VisionSource visionSource in VisionSources)
            {
                if (visionSource.Owner is Owner.Player or Owner.Ally || IsPointSimulated(visionSource.Position))
                    SimulatedVisionSources.Add(visionSource);
                else
                    IdleVisionSources.Add(visionSource);
                
                switch (visionSource.Owner)
                {
                    case Owner.Enemy:
                        EnemyVisionSources.Add(visionSource);
                        break;
                    case Owner.Neutral:
                        continue;
                }
                PlayerVisionSources.Add(visionSource);
                PlayerBounds.Add(visionSource.Bounds);
                PlayerSimulationBounds.Add(visionSource.SimulationBounds);
            }
            
            foreach (VisionSource visionSource in SimulatedVisionSources)
                await visionSource.Recalculate();
            foreach (VisionSource visionSource in IdleVisionSources)
                visionSource.Mute();
            
            await _fogOfWar.Recalculate();
            _isRecalculating = false;
        }
    }
}