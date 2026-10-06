using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Composite", order = 0)]
    public class CompositeStatus : StatusType
    {
        [SerializeField] private StatusType[] _subStatuses;
        
        public override void OnAdd(Status status)
        {
            foreach (StatusType subStatus in _subStatuses)
            {
                subStatus.OnAdd(status);
            }
        }

        public override void OnUpdate(Status status)
        {
            foreach (StatusType subStatus in _subStatuses)
            {
                subStatus.OnUpdate(status);
            }
        }

        public override void OnRemove(Status status)
        {
            foreach (StatusType subStatus in _subStatuses)
            {
                subStatus.OnRemove(status);
            }
        }
    }
}