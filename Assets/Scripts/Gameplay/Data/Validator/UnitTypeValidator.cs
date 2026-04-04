using Gameplay.Data.Units;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Type", order = 0)]
    public class UnitTypeValidator : UnitValidator
    {
        [SerializeField] private UnitType _type;
        [SerializeField] private bool _required;
        
        public override bool IsValid(Unit actor, Unit target) => (target.Type == _type) == _required;
    }
}