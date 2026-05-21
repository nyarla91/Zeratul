using System;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolNot : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<bool> _a;
        
        public override bool Value => ! _a.Value;

        private void OnValidate()
        {
            gameObject.name = $"Not {_a?.name}";
        }
    }
}