using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Caster", order = 0)]
    public class CasterValidator : UnitValidator
    {
        [SerializeField] private bool _isCaster;
        
        public override bool IsValid(Unit actor, Unit target)
        {
            return _isCaster == (actor == target);
        }
    }
}