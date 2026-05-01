using Extentions;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Take Ownership", order = 0)]
    public class TakeOwnershipStatus : StatusType
    {
        public override void OnAdd(Status status)
        {
            status.Host.Alliance.AddOwner(status, status.Instigator.Alliance.CurrentOwner);
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            status.Host.Alliance.RemoveOwner(status);
        }
    }
}