using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Detected", order = 0)]
    public class DetectedStatus : StatusType
    {
        public override void OnAdd(Status status)
        {
            status.Host.Visibility.Detect(status);
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            status.Host.Visibility.StopDetecting(status);
        }
    }
}