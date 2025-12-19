using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Log", order = 0)]
    public class LogStatus : StatusType
    {
        public override void OnAdd(Status status)
        {
            Debug.Log($"{this} was added to {status.Host} by {status.Instigator}");
        }

        public override void OnUpdate(Status status)
        {
            Debug.Log($"{status.Host} has {this}. {status.FramesLeft} frames left");
        }

        public override void OnRemove(Status status)
        {
            Debug.Log($"{this} was removed from {status.Host}");
        }
    }
}