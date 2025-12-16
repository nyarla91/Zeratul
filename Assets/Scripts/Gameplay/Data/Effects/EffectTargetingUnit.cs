using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    public abstract class EffectTargetingUnit : ScriptableObject
    {
        public abstract void Apply(Unit caster, Unit target);
    }
}