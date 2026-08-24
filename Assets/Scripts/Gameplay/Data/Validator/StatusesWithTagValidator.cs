using Gameplay.Data.Statuses;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Statuses With Tag", order = 0)]
    public class StatusesWithTagValidator : UnitValidator
    {
        [SerializeField] private StatusTag _tag;
        [SerializeField] private int _minDuration;
        [SerializeField] private int _maxDuration;
        

        public override bool IsValid(Unit actor, Unit target)
        {
            foreach (IStatusInfo status in target.Statuses.StatusesInfo)
            {
                if (status.Type.Tag == _tag && status.FramesLeft >= _minDuration && status.FramesLeft <= _maxDuration)
                    return true;
            }
            return false;
        }
    }
}