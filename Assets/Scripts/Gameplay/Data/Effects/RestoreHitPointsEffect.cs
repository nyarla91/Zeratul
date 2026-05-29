using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Restore Hit Points", order = 0)]
    public class RestoreHitPointsEffect : EffectTargetingUnit
    {
        [SerializeField] private int _value;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Life?.RestoreHitPoints(_value);
        }
    }
}