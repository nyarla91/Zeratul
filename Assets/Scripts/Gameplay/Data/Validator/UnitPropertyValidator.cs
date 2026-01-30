using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public abstract class UnitPropertyValidator : UnitValidator
    {
        [SerializeField] private PropertyComparator _propertyComparator;
        [SerializeField] private int _targetValue;

        private Func<int, int, bool> Comparison
        {
            get
            {
                return _propertyComparator switch
                {
                    PropertyComparator.Equals => (a, b) => a == b,
                    PropertyComparator.NotEquals => (a, b) => a != b,
                    PropertyComparator.LessThan => (a, b) => a < b,
                    PropertyComparator.LessThanOrEquals => (a, b) => a <= b,
                    PropertyComparator.MoreThan => (a, b) => a > b,
                    PropertyComparator.MoreThanOrEquals => (a, b) => a >= b,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        public override bool IsValid(Unit actor, Unit target)
        {
            return Comparison(GetUnitProperty(target), _targetValue);
        }

        protected abstract int GetUnitProperty(Unit unit);

        private enum PropertyComparator
        {
            Equals,
            NotEquals,
            LessThan,
            LessThanOrEquals,
            MoreThan,
            MoreThanOrEquals,
        }
    }
}