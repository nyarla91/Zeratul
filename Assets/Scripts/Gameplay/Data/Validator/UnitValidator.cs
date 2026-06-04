using System;
using _Core;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public abstract class UnitValidator : ScriptableObject
    {
        [SerializeField] private string _invalidMessage;
        [SerializeField] private bool _not;

        protected string RawInvalidMessage => _invalidMessage;
        public bool Not => _not;

        public abstract bool IsValid(Unit actor, Unit target);

        public virtual string GetInvalidMessage(Unit actor, Unit target)
        {
            return RawInvalidMessage;
        }
    }
    
    [Serializable]
    public struct UnitValidatorGroup
    {
        [SerializeField] private UnitValidator[] _unitValidators;
        
        public bool IsInvalid(Unit actor, Unit unit, out string invalidMessage)
        {
            invalidMessage = null;
            if (_unitValidators == null || _unitValidators.Length == 0)
                return false;
            foreach (UnitValidator validator in _unitValidators)
            {
                if ( ! validator)
                    continue;
                if (validator.IsValid(actor, unit) != validator.Not)
                    continue;
                invalidMessage = validator.GetInvalidMessage(actor, unit);
                return true;
            }
            return false;
        }
        
        public bool IsValid(Unit actor, Unit target, out string invalidMessage)
        {
            return ! IsInvalid(actor, target, out invalidMessage);
        }
        
        public bool IsInvalid(Unit actor, Unit unit) => IsInvalid(actor, unit, out _);
        
        public bool IsValid(Unit actor, Unit target) => IsValid(actor, target, out _);

        public override string ToString()
        {
            return _unitValidators.Length == 0 ? "" : $"({_unitValidators.Enumerate(", ", "", v => v.name)})";
        }
    }
}