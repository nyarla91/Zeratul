using System;
using Cysharp.Threading.Tasks;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionSet<T> : SchemeAction
    {
        [SerializeField] private SchemeVariable<T> _variable;
        [SerializeField] private SchemeValue<T> _value;
        
        public override UniTask Act()
        {
            _variable.Set(_value.Value);
            return UniTask.CompletedTask;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Set {_variable?.name} to {_value?.name}";
            
        }
    }
}