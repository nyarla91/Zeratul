using _Core.Input;
using Gameplay.Player;
using Gameplay.Vision;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class EnemyVisionViewToggle : GameplayViewToggle
    {
        [Inject] private FogOfWar FogOfWar { get; set; }
        
        protected override bool IsOn => FogOfWar.DisplayEnemyVision;

        protected override InputBinding GetInputBinding(PlayerInput playerInput) => playerInput.ToggleEnemyVision;

        protected override void ToggleOn() => FogOfWar.DisplayEnemyVision = true;
        protected override void ToggleOff() => FogOfWar.DisplayEnemyVision = false;
    }
}
