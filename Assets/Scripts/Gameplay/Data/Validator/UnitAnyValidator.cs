using System.Linq;
using Gameplay.Units;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Any", order = 0)]
    public class UnitAnyValidator : UnitValidator
    {
        [SerializeField] private UnitValidator[] _validators;
        
        public override bool IsValid(Unit actor, Unit target)
        {
            return _validators.Length == 0 || _validators.Any(v => v.IsValid(actor, target));
        }
    }
}