using _Core.Input;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class TacticalPauseToggle : GameplayViewToggle
    {
        [Inject] private TacticalPause TacticalPause { get; set; }

        protected override bool IsOn => TacticalPause.IsPausedFrom(this);
        
        protected override InputBinding GetInputBinding(PlayerInput playerInput) => playerInput.ToggleTacticalPause;
        
        protected override void ToggleOn() => TacticalPause.Pause(this);
        protected override void ToggleOff() => TacticalPause.Unpause(this);
    }
}
