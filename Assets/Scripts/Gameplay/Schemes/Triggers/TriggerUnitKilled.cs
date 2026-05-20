using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerUnitKilled : TriggerUnitEvent
    {
        protected override void Subscribe(Unit unit)
        {
            unit.Killed += () => OutAndTrigger(unit);
        }

        private void OnValidate()
        {
            gameObject.name = "Unit killed";
        }
    }
}