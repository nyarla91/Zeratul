using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitTookDamage : TriggerUnitEvent
    {
        [SerializeField] private VariableInt _outDamage;

        protected override void Subscribe(Unit unit)
        {
            if (unit.Life == null)
                return;

            unit.Life.DamageTaken += (damage) =>
            {
                _outDamage?.Set(damage);
                OutAndTrigger(unit);
            };
        }

        private void OnValidate()
        {
            gameObject.name = $"Unit {Out?.name} took damage {_outDamage?.name}";
        }
    }
}
