using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Kill", order = 0)]
    public class KillEffect : EffectTargetingUnit
    {
        public override void Apply(Unit caster, Unit target)
        {
            target.Kill();
        }
    }
}