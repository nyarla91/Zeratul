using System;
using Extentions;
using Gameplay.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Vision
{
    public class VisionMap : MonoBehaviour
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private VisionArea _playerArea;
        [SerializeField] private VisionArea _neutralArea;
        [SerializeField] private VisionArea _enemyArea;
        
        public VisionArea PlayerArea => _playerArea;
        public VisionArea EnemyArea => _enemyArea;
        public VisionArea NeutralArea => _neutralArea;

        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Awake()
        {
            PlayerArea.Init(Owner.Player);
            NeutralArea.Init(Owner.Neutral);
            EnemyArea.Init(Owner.Enemy);
        }
        
        public VisionArea GetAreaForOwner(Owner owner)
        {
            return owner switch
            {
                Owner.Player => PlayerArea,
                Owner.Ally => PlayerArea,
                Owner.Neutral => NeutralArea,
                Owner.Enemy => EnemyArea,
                _ => throw new ArgumentOutOfRangeException(nameof(owner), owner, null)
            };
        }
    }
}