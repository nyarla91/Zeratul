using Extentions.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay
{
    public class PauseCrutch : MonoBehaviour
    {
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (GamePause.IsPaused)
                    GamePause.Unpause(this);
                else
                    GamePause.Pause(this);
            }
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (TacticalPause.IsPaused)
                    TacticalPause.Unpause(this);
                else
                    TacticalPause.Pause(this);
            }
        }
    }
}