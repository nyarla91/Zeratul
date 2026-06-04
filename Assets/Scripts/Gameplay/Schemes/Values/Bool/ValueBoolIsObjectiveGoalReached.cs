using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolIsObjectiveGoalReached : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<_Core.Objective> _objective;

        public override bool Value => _objective.Value.Counter >= _objective.Value.Goal;

        private void OnValidate()
        {
            gameObject.name = $"(Is goal of {_objective?.name} reached)";
        }
    }
}