using System;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionSet<T> : SchemeAction
    {
        [SerializeField] private SchemeVariable<T> _variable;
        [SerializeField] private SchemeValue<T> _value;
        
        public override void Act()
        {
            _variable.Set(_value.Value);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Set ({_variable?.name}) to ({_value?.name})";
            
        }
    }
}