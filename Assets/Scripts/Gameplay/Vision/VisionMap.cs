using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionMap : MonoBehaviour
    {
        [SerializeField] private VisionArea _playerArea;
        [SerializeField] private VisionArea _enemyArea;

        public VisionArea PlayerArea => _playerArea;
        public VisionArea EnemyArea => _enemyArea;
    }
}