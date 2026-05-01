using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Locked", order = 0)]
    public class LockedStatus : StatusType
    {
        public override void OnAdd(Status status)
        {
            status.Host.Abilities.Lock(status);
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            status.Host.Abilities.Unlock(status);
        }
    }
}