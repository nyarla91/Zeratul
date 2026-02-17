using System;
using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public class StatusSprite : MonoBehaviour
    {
        [SerializeField] private SpriteLayeringConfig _config;
        private SpriteRenderer _spriteRenderer;

        private SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();

        private void Awake()
        {
            SpriteRenderer.sortingOrder = _config.StatusOrder;
        }
    }
}