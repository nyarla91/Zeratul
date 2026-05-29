using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Restore Shield Points", order = 0)]
    public class RestoreShieldPointsEffect : EffectTargetingUnit
    {
        [SerializeField] private int _value;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Life?.RestoreShieldPoints(_value);
        }
    }
}