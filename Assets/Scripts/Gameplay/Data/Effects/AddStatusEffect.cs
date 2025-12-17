using Gameplay.Data.Statuses;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Add Status", order = 0)]
    public class AddStatusEffect : EffectTargetingUnit
    {
        [SerializeField] private StatusType _statusType;
        [SerializeField] private int _duration;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Statuses.AddStatus(_statusType, caster, _duration);
        }
    }
}