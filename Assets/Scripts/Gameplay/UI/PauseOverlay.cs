using System;
using Extentions.Pause;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class PauseOverlay : MonoBehaviour
    {
        [SerializeField] private Image _gamePauseOverlay;
        [SerializeField] private Color _gamePauseColor;
        [SerializeField] private Image _tacticalPauseOverlay;
        [SerializeField] private Color _tacticalPauseColor;
        
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Update()
        {
            _gamePauseOverlay.color = GamePause.IsPaused ? _gamePauseColor : Color.clear;
            _tacticalPauseOverlay.color = TacticalPause.IsPaused ? _tacticalPauseColor : Color.clear;
        }
    }
}