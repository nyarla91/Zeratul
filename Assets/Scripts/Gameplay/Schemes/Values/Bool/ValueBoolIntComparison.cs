using System;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolIntComparison : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<int> _a;
        [SerializeField] private SchemeValue<int> _b;
        [SerializeField] private Operation _operation;

        public override bool Value
        {
            get
            {
                return _operation switch
                {
                    Operation.Equals => _a.Value == _b.Value,
                    Operation.NotEquals => _a.Value != _b.Value,
                    Operation.More => _a.Value > _b.Value,
                    Operation.MoreOrEquals => _a.Value >= _b.Value,
                    Operation.Less => _a.Value < _b.Value,
                    Operation.LessOrEquals => _a.Value <= _b.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private void OnValidate()
        {
            string operationSign = _operation switch
            {
                Operation.Equals => "==",
                Operation.NotEquals => "!=",
                Operation.More => ">",
                Operation.MoreOrEquals => ">=",
                Operation.Less => "<",
                Operation.LessOrEquals => "<=",
                _ => throw new ArgumentOutOfRangeException()
            };
            gameObject.name = $"({_a?.name} {operationSign} {_b?.name})";
        }

        private enum Operation
        {
            Equals,
            NotEquals,
            More,
            MoreOrEquals,
            Less,
            LessOrEquals
        }
    }
}