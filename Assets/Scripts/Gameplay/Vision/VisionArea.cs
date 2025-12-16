using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionArea : MonoBehaviour
    {
        public void AttachSightArea(Transform sightArea)
        {
            sightArea.SetParent(transform);
            sightArea.gameObject.layer = gameObject.layer;
        }
    }
}