using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public class StatusSprite : StatusRenderer
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private bool _overrideSortingOrder = true;
        
        private void Awake()
        {
            if (_overrideSortingOrder)
                _spriteRenderer.sortingOrder = _config.StatusOrder;
        }

        protected override void UpdateVisibility(bool isVisible) => _spriteRenderer.enabled = isVisible;
    }
}