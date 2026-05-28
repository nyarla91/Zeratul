using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Vision
{
    public class EnemyVisionViewToggle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Inject] private PlayerInput PlayerInput { get; set; }
        
        private void Awake()
        {
            _spriteRenderer.enabled = false;
            PlayerInput.ToggleEnemyVision.Performed += Toggle;
        }

        private void Toggle()
        {
            _spriteRenderer.enabled = ! _spriteRenderer.enabled;
        }
    }
}