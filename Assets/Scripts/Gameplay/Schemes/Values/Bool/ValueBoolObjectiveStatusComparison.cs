using _Core;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolObjectiveStatusComparison : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<_Core.Objective> _objective;
        [SerializeField] private ObjectiveStatus _status;

        public override bool Value => _objective.Value.Status == _status;

        private void OnValidate()
        {
            gameObject.name = $"(Is {_objective?.name} {_status:G})";
        }
    }
}