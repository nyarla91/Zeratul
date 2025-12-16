using System;
using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public abstract class UnitValidator : ScriptableObject
    {
        public abstract bool IsValid(Unit actor, Unit target);
    }
    
    [Serializable]
    public struct UnitValidatorGroup
    {
        [SerializeField] private UnitValidator[] _unitValidators;
        
        public bool IsValid(Unit actor, Unit unit)
            => _unitValidators.Length ==  0 || _unitValidators.All(v => v.IsValid(actor, unit));
        
        public bool IsInvalid(Unit actor, Unit unit)
            => _unitValidators.Length > 0 && _unitValidators.Any(v => ! v.IsValid(actor,unit));
    }
}