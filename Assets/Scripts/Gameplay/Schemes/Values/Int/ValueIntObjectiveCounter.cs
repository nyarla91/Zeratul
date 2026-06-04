using UnityEngine;

namespace Gameplay.Schemes.Values.Int
{
    public class ValueIntObjectiveCounter : SchemeValue<int>
    {
        [SerializeField] private SchemeValue<_Core.Objective> _objective;

        public override int Value => _objective.Value.Counter;

        private void OnValidate()
        {
            gameObject.name = $"(Counter of {_objective?.name})";
        }
    }
}