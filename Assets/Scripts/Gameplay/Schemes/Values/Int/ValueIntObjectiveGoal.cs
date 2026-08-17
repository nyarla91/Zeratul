using UnityEngine;

namespace Gameplay.Schemes.Values.Int
{
    public class ValueIntObjectiveGoal : SchemeValue<int> 
    {
        [SerializeField] private SchemeValue<_Core.Objective> _objective;

        public override int Value => _objective.Value.Goal;

        private void OnValidate()
        {
          gameObject.name = $"(Goal of {_objective?.name})";
        } 
    }
}