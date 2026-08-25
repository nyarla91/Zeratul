using System;
using Gameplay.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View.StatusRendering
{
    public class StatusSprite : StatusRenderer
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _overrideSortingOrder = true;
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        
        private void Awake()
        {
            if (_overrideSortingOrder)
                _renderer.sortingOrder = _config.StatusOrder;
        }

        protected override void UpdateVisibility(bool isVisible) => _renderer.enabled = isVisible;

        private void Update()
        {
            if (_animator)
                _animator.speed = TacticalPause.IsPaused ? 0 : 1;
        }
    }
}