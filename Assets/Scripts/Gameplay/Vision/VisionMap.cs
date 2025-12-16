using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionMap : MonoBehaviour
    {
        [SerializeField] private VisionArea _playerArea;
        [SerializeField] private VisionArea _enemyArea;

        public void AttachSightArea(Transform sightArea, bool ownedByPlayer)
        {
            if (ownedByPlayer)
                _playerArea.AttachSightArea(sightArea);
            else
                _enemyArea.AttachSightArea(sightArea);
        } 
    }
}