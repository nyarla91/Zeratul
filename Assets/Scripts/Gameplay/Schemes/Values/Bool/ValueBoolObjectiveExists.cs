using System;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolObjectiveExists : SchemeValue<bool>
    {
        [SerializeField] private VariableObjective _objective;

        public override bool Value => _objective.Value != null;

        private void OnValidate()
        {
            gameObject.name = $"({_objective?.name}) exists";
        }
    }
}