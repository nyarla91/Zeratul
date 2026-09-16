using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitLostHitPoints : TriggerUnitEvent
    {
        [SerializeField] private VariableInt _outLostHitPoints;

        protected override void Subscribe(Unit unit)
        {
            if (unit.Life == null)
                return;

            unit.Life.HitPointsLost += (lostHitPoints) =>
            {
                _outLostHitPoints?.Set(lostHitPoints);
                OutAndTrigger(unit);
            };
        }

        private void OnValidate()
        {
            gameObject.name = $"Unit {Out?.name} lost hit points {_outLostHitPoints?.name}";
        }
    }
}
