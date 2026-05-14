using Gameplay.Data.Statuses;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Remove Status", order = 0)]
    public class RemoveStatusEffect : EffectTargetingUnit
    {
        [SerializeField] private StatusType _statusType;

        public override void Apply(Unit caster, Unit target)
        {
            target.Statuses.RemoveStatus(_statusType);
        }
    }
}