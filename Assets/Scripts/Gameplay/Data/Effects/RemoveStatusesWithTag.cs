using Gameplay.Data.Statuses;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Remove Statuses With Tag", order = 0)]
    public class RemoveStatusesWithTag : EffectTargetingUnit
    {
        [SerializeField] private StatusTag _tag;
        [SerializeField] private int _minDuration;
        [SerializeField] private int _maxDuration;
        
        public override void Apply(Unit caster, Unit target)
        {
            foreach (IStatusInfo status in target.Statuses.StatusesInfo)
            {
                if (status.Type.Tag != _tag || status.FramesLeft < _minDuration || status.FramesLeft > _maxDuration)
                    continue;
                target.Statuses.RemoveStatus(status.Type);
            }
        }
    }
}