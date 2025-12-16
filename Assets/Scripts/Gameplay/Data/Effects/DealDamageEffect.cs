using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effeccts/Deal Damage", order = 0)]
    public class DealDamageEffect : EffectTargetingUnit
    {
        [SerializeField] private int _damage;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Life.TakeDamage(_damage);
        }
    }
}