using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Restore Energy Points", order = 0)]
    public class RestoreEnergyPointsEffect : EffectTargetingUnit
    {
        [SerializeField] private int _value;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Abilities.RestoreEnergyPoints(_value);
        }
    }
}