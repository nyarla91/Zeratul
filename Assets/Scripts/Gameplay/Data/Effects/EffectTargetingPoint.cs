using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    public abstract class EffectTargetingPoint : EffectTargetingUnit
    {
        public override void Apply(Unit caster, Unit target) => Apply(caster, target.transform.position);

        public abstract void Apply(Unit caster, Vector2 target);
    }
}