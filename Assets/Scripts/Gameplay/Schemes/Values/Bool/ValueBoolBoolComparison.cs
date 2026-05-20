using System;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolBoolComparison : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<bool> _a;
        [SerializeField] private SchemeValue<bool> _b;
        [SerializeField] private Operation _operation;

        public override bool Value
        {
            get
            {
                return _operation switch
                {
                    Operation.And => _a.Value && _b.Value,
                    Operation.Or => _b.Value || _a.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private void OnValidate()
        {
            string operationSign = _operation switch
            {
                Operation.And => "&&",
                Operation.Or => "||",
                _ => throw new ArgumentOutOfRangeException()
            };
            gameObject.name = $"({_a?.name} {operationSign} {_b?.name})";
        }

        private enum Operation
        {
            And,
            Or
        }
    }
}