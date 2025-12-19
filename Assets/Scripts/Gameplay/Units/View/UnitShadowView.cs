using System;
using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitShadowView : MonoBehaviour
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _animationSpriteRenderer; 
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            if ( ! _unit.Type.Movement.IsAir)
                Destroy(gameObject);
            _spriteRenderer.sortingOrder = _config.ShadowOrder;
        }

        private void Update()
        {
            _spriteRenderer.sprite = _animationSpriteRenderer.sprite;
        }
    }
}