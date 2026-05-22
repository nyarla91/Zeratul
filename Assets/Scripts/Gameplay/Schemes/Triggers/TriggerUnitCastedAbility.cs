using System;
using Gameplay.Data.Abilities;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitCastedAbility : TriggerUnitEvent
    {
        [SerializeField] private AbilityType _abilityType;
        [SerializeField] private SchemeVariable<Unit> _outTargetUnit;
        [SerializeField] private SchemeVariable<Vector2> _outTargetPoint;


        protected override void Subscribe(Unit unit)
        {
            unit.Abilities.CastedAbility += (ability, orderTarget) =>
            {
                if (ability != _abilityType)
                    return;
                if (orderTarget.Unit)
                    _outTargetUnit?.Set(orderTarget.Unit);
                if (orderTarget.Point != default)
                    _outTargetPoint?.Set(orderTarget.Point);
                OutAndTrigger(unit);
            };
        }

        private void OnValidate()
        {
            gameObject.name = $"Unit casted ({_abilityType?.name})";
        }
    }
}