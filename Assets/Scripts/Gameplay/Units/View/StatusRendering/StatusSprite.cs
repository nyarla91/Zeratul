using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public class StatusSprite : MonoBehaviour
    {
        [SerializeField] private StatusRenderer _statusRenderer;
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private bool _alwaysVisible;
        
        private SpriteRenderer _spriteRenderer;

        private SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();

        private void Awake()
        {
            SpriteRenderer.sortingOrder = _config.StatusOrder;
        }

        private void Update()
        {
            _spriteRenderer.enabled = _alwaysVisible || _statusRenderer.Status.Host.Visibility.IsVisibleToPlayer; 
        }
    }
}