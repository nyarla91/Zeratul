using System;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitShadowView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _animationSpriteRenderer; 
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            if ( ! _unit.Type.Movement.IsAir)
                Destroy(gameObject);
        }

        private void Update()
        {
            _spriteRenderer.sprite = _animationSpriteRenderer.sprite;
        }
    }
}