using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public class UnitAnyValidator : UnitValidator
    {
        [SerializeField] private UnitValidator[] _validators;
        
        public override bool IsValid(Unit actor, Unit target)
        {
            return _validators.Length == 0 || _validators.Any(v => v.IsValid(actor, target));
        }
    }
}