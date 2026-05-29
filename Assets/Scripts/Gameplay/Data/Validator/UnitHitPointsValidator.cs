using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Hit Points", order = 0)]
    public class UnitHitPointsValidator : UnitPropertyValidator
    {
        protected override int GetUnitProperty(Unit unit) => unit.Life?.HitPoints ?? 0;
    }
}