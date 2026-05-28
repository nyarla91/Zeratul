using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class PauseOverlay : MonoBehaviour
    {
        [SerializeField] private Image _tacticalPauseOverlay;
        [SerializeField] private Color _tacticalPauseColor;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Update()
        {
            _tacticalPauseOverlay.color = TacticalPause.IsPaused ? _tacticalPauseColor : Color.clear;
        }
    }
}