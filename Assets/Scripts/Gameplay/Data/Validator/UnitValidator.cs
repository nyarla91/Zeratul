using System;
using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public abstract class UnitValidator : ScriptableObject
    {
        public abstract bool IsValid(Unit unit);
    }
    
    [Serializable]
    public struct UnitValidatorGroup
    {
        [SerializeField] private UnitValidator[] _unitValidators;
        
        public bool IsValid(Unit unit) => _unitValidators.All(v => v.IsValid(unit));
        
        public bool IsInvalid(Unit unit) => _unitValidators.Any(v => ! v.IsValid(unit));
    }
}