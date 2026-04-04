using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    public abstract class PointValidator : UnitValidator
    {
        public override bool IsValid(Unit actor, Unit target) => IsValid(actor, target.Position);

        public abstract bool IsValid(Unit actor, Vector2 point);
    }
}