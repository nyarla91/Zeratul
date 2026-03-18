using Gameplay.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Vision
{
    public class VisionMap : MonoBehaviour
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private VisionArea _playerArea;
        [SerializeField] private VisionArea _enemyArea;
        
        public VisionArea PlayerArea => _playerArea;
        public VisionArea EnemyArea => _enemyArea;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Awake()
        {
            PlayerArea.Init(true);
            EnemyArea.Init(false);
        }
        
        public VisionArea GetAreaForOwner(bool ownedByPlayer) => ownedByPlayer ? PlayerArea : EnemyArea;
    }
}