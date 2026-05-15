using Extentions;
using Gameplay.Data.Units;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Spawn Unit", order = 0)]
    public class SpawnUnitEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private UnitType _unitType;
        [SerializeField] private EffectTargetingUnit[] _spawnedUnitEffects;
        
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        
        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);
            
            float lookAngle = (caster.Position.DirectionTo(target) / Isometry.Scale).ToDegrees();
            UnitSpawnInfo spawnInfo = new(caster.Alliance.CurrentOwner, lookAngle, null);
            
            Unit spawnedUnit = UnitSpawner.Spawn(target, _unitType, -1, spawnInfo);
            
            foreach (EffectTargetingUnit effect in _spawnedUnitEffects)
            {
                effect.Apply(caster, spawnedUnit);
            }
        }
    }
}