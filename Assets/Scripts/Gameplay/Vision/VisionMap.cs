using System;
using Extentions;
using Extentions.Pause;
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

        private Timer _recalculationTimer;
        
        public VisionArea PlayerArea => _playerArea;
        public VisionArea EnemyArea => _enemyArea;
        public ITimerWrap RecalculationTimer => _recalculationTimer;
        
        [Inject] private IPauseRead PauseRead { get; set; }

        private void Awake()
        {
            _recalculationTimer = new Timer(this, _config.RecalculationPeriod, PauseRead, true);
            _recalculationTimer.Start();
            
            PlayerArea.Init(true);
            EnemyArea.Init(false);
        }
    }
}