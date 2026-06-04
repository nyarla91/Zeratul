using System;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionSetObjectiveCounter : SchemeAction
    {
        [SerializeField] private VariableObjective _objective;
        [SerializeField] private SchemeValue<int> _counter;
        
        public override void Act()
        {
            _objective.Value.UpdateCurrentCounter(_counter.Value);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Set {_objective?.name} counter to {_counter?.name}";
        }
    }
}