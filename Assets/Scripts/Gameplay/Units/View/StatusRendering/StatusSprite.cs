using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public class StatusSprite : StatusRenderer
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool _overrideSortingOrder = true;
        
        private void Awake()
        {
            if (_overrideSortingOrder)
                _renderer.sortingOrder = _config.StatusOrder;
        }

        protected override void UpdateVisibility(bool isVisible) => _renderer.enabled = isVisible;
    }
}