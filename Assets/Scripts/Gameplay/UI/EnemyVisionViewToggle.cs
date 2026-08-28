using _Core.Input;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class EnemyVisionViewToggle : GameplayViewToggle
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        protected override bool IsOn => _spriteRenderer.enabled;

        protected override void Awake()
        {
            _spriteRenderer.enabled = false;
            base.Awake();
        }

        protected override InputBinding GetInputBinding(PlayerInput playerInput) => playerInput.ToggleEnemyVision;

        protected override void ToggleOn() => _spriteRenderer.enabled = true;
        protected override void ToggleOff() => _spriteRenderer.enabled = false;
    }
}
