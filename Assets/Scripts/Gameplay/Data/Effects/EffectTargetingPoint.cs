using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    public abstract class EffectTargetingPoint : ScriptableObject
    {
        public abstract void Apply(Unit caster, Vector2 target);
    }
}