using System;
using _Core;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionSetObjectiveStatus : SchemeAction
    {
        [SerializeField] private VariableObjective _objective;
        [SerializeField] private ObjectiveStatus _status;

        public override void Act()
        {
            _objective.Value.Status = _status;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Set {_objective?.name} status to {_status:G}";
        }
    }
}