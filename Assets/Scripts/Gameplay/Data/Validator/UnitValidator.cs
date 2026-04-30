using System;
using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public abstract class UnitValidator : ScriptableObject
    {
        [SerializeField] private string _invalidMessage;
        [SerializeField] private bool _not;

        public string InvalidMessage => _invalidMessage;
        public bool Not => _not;

        public abstract bool IsValid(Unit actor, Unit target);
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
                invalidMessage = validator.InvalidMessage;
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
    }
}