using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Teleport", order = 0)]
    public class TeleportEffect : EffectTargetingPoint
    {
        public override void Apply(Unit caster, Vector2 target)
        {
            caster.Movement.Teleport(target);
        }
    }
}