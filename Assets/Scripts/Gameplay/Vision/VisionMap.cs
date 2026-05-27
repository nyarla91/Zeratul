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
        public HashSet<VisionSource> PlayerVisionSources { get; private set; }
        public HashSet<VisionSource> EnemyVisionSources { get; private set; }
        public HashSet<Bounds> PlayerBounds { get; private set; }
        public HashSet<Bounds> PlayerSimulationBounds { get; private set; }

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
            foreach (VisionSource visionSource in VisionSources)
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
                if (bounds.Contains(point))
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

        public void RemoveSource(VisionSource source)
        {
            VisionSources.Remove(source);
        }

        private async UniTask Recalculate()
        {
            _isRecalculating = true;
            PlayerVisionSources = VisionSources
                .Where(v => v.Owner is Owner.Player or Owner.Ally)
                .ToHashSet();

            EnemyVisionSources = VisionSources
                .Where(v => v.Owner is Owner.Enemy)
                .ToHashSet();

            PlayerBounds = PlayerVisionSources
                .Select(v => v.Bounds)
                .ToHashSet();

            PlayerSimulationBounds = PlayerVisionSources
                .Select(v => v.SimulationBounds)
                .ToHashSet();
            
            foreach (VisionSource visionSource in VisionSources)
            {
                await visionSource.Recalculate(IsPointSimulated(visionSource.Position));
            }
            
            await _fogOfWar.Recalculate();
            _isRecalculating = false;
        }
    }
}