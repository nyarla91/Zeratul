using Gameplay.Data.Abilities;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Ability", order = 0)]
    public class UnitAbilityValidator : UnitValidator
    {
        [SerializeField] private AbilityType _ability;
        
        public override bool IsValid(Unit actor, Unit target)
        {
            return target.Abilities.GetAbility(_ability) != null;
        }
    }
}