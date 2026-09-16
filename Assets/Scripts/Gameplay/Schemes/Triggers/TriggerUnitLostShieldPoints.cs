using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitLostShieldPoints : TriggerUnitEvent
    {
        [SerializeField] private VariableInt _outLostShieldPoints;

        protected override void Subscribe(Unit unit)
        {
            if (unit.Life == null)
                return;

            unit.Life.ShieldPointsLost += (lostShieldPoints) =>
            {
                _outLostShieldPoints?.Set(lostShieldPoints);
                OutAndTrigger(unit);
            };
        }

        private void OnValidate()
        {
            gameObject.name = $"Unit {Out?.name} lost shield points {_outLostShieldPoints?.name}";
        }
    }
}
