using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class PauseOverlay : MonoBehaviour
    {
        [SerializeField] private Image _tacticalPauseOverlay;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Update()
        {
            _tacticalPauseOverlay.enabled = TacticalPause.IsPausedSelf;
        }
    }
}