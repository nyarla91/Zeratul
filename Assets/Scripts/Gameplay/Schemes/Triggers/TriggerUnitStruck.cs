using System;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitStruck : TriggerUnitEvent
    {
        [SerializeField] private VariableUnit _outTarget;
        
        protected override void Subscribe(Unit unit)
        {
            unit.Attack.Struck += (target) =>
            {
                _outTarget?.Set(target);
                OutAndTrigger(unit);
            };
        }

        private void OnValidate()
        {
            gameObject.name = $"Unit {Out?.name} struck {_outTarget?.name}";
        }
    }
}