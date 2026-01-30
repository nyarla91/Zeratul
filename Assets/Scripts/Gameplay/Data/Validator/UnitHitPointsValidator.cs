using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public class UnitHitPointsValidator : UnitPropertyValidator
    {
        protected override int GetUnitProperty(Unit unit) => unit.Life.HitPoints;
    }
}