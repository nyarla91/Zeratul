using Extentions.Pause;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Cloaked", order = 0)]
    public class CloakedStatus : StatusType
    {
        public override void OnAdd(Status status)
        {
            status.Host.Visibility.Cloak(status);
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            status.Host.Visibility.Decloak(status);
        }
    }
}