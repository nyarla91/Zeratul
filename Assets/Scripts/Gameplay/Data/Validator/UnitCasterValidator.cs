using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Caster", order = 0)]
    public class UnitCasterValidator : UnitValidator
    {
        public override bool IsValid(Unit actor, Unit target) => actor == target;
    }
}