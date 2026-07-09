using _Core;
using Codice.CM.Common;
using Gameplay.Data.Units;
using Gameplay.Units;
using UnityEngine;
using Zenject;
using OrderType = Gameplay.Data.Orders.OrderType;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Spawn Unit", order = 0)]
    public class SpawnUnitEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private UnitType _unitType;
        [SerializeField] private EffectTargetingUnit[] _spawnedUnitEffects;
        [SerializeField] private OrderType _orderToSpawned;
        
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        
        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);
            
            float lookAngle = (caster.Position.DirectionTo(target) / Isometry.Scale).ToDegrees();
            UnitSpawnInfo spawnInfo = new(caster.Alliance.CurrentOwner, lookAngle, null);

            Vector2 spawnPoint = target;
            if (_orderToSpawned)
            {
                float offset = (caster.Type.Size + _unitType.Size) / 2;
                Vector2 direction = caster.Position.DirectionTo(spawnPoint);
                float angle = direction.ToDegrees();
                spawnPoint = caster.Position + offset * Isometry.Multiplier(angle) * direction;
            }
            Unit spawnedUnit = UnitSpawner.Spawn(spawnPoint, _unitType, -1, spawnInfo);

            if (_orderToSpawned)
            {
                Order order = new Order(_orderToSpawned, spawnedUnit, OrderTarget.FromPoint(target));
                spawnedUnit.Orders.IssueOrder(order, false);
            }
            
            foreach (EffectTargetingUnit effect in _spawnedUnitEffects)
            {
                effect.Apply(caster, spawnedUnit);
            }
        }
    }
}