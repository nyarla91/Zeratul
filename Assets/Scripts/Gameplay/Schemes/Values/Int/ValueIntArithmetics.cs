using System;
using UnityEngine;

namespace Gameplay.Schemes.Values.Int
{
    public class ValueIntArithmetics : SchemeValue<int>
    {
        [SerializeField] private SchemeValue<int> _a;
        [SerializeField] private SchemeValue<int> _b;
        [SerializeField] private Operation _operation;

        public override int Value
        {
            get
            {
                return _operation switch
                {
                    Operation.Add => _a.Value + _b.Value,
                    Operation.Subtract => _a.Value - _b.Value,
                    Operation.Multiply => _a.Value * _b.Value,
                    Operation.Divide => _a.Value / _b.Value,
                    Operation.Modulo => _a.Value % _b.Value,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private void OnValidate()
        {
            string operationSign = _operation switch
            {
                Operation.Add => "+",
                Operation.Subtract => "-",
                Operation.Multiply => "*",
                Operation.Divide => "/",
                Operation.Modulo => "%",
                _ => throw new ArgumentOutOfRangeException()
            };
            gameObject.name = $"({_a?.name} {operationSign} {_b?.name})";
        }


        private enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo
        }
    }
}