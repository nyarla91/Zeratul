using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class TacticalPauseControl : MonoBehaviour
    {
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private PlayerInput PlayerInput { get; set; }

        private void Awake()
        {
            PlayerInput.ToggleTacticalPause.Performed += TogglePause;
        }

        public void TogglePause()
        {
            if (TacticalPause.IsPausedFrom(this))
                TacticalPause.Unpause(this);
            else
                TacticalPause.Pause(this);
        }
    }
}