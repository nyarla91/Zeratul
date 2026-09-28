using System;
using _Core;
using Cysharp.Threading.Tasks;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionSetObjectiveStatus : SchemeAction
    {
        [SerializeField] private VariableObjective _objective;
        [SerializeField] private ObjectiveStatus _status;

        public override UniTask Act()
        {
            _objective.Value.Status = _status;
            return UniTask.CompletedTask;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Set {_objective?.name} status to {_status:G}";
        }
    }
}