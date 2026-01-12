using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Ownership", order = 0)]
    public class OwnershipStatus : StatusType
    {
        [SerializeField] private bool _isFriendly;
        
        public override void OnAdd(Status status)
        {
            bool ownedByPlayer = status.Instigator.Ownership.OwnedByPlayer == _isFriendly;
            status.Host.Ownership.AddOwner(status, ownedByPlayer);
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            status.Host.Ownership.RemoveOwner(status);
        }
    }
}