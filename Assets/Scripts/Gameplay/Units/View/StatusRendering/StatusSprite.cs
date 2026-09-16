using System;
using _Core;
using DG.Tweening;
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
        [SerializeField] private bool _popAnimation;
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        
        private void Awake()
        {
            if (_overrideSortingOrder)
                _renderer.sortingOrder = _config.StatusOrder;
        }

        protected override void UpdateVisibility(bool isVisible) => _renderer.enabled = isVisible;

        public override void OnSpawn()
        {
            base.OnSpawn();
            SpriteRenderer spriteRenderer = _renderer as SpriteRenderer;
            if ( ! spriteRenderer)
                return;
            spriteRenderer.color = spriteRenderer.color.WithA(0);
            spriteRenderer.DOFade(1, 0.5f);
            Vector3 finiteScale = transform.localScale;
            transform.localScale *= 3;
            transform.DOScale(finiteScale, 0.5f);
        }

        private void Update()
        {
            if (_animator)
                _animator.speed = TacticalPause.IsPaused ? 0 : 1;
        }
    }
}