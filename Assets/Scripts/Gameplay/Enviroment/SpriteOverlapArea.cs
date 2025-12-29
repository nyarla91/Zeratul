using System;
using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Enviroment
{
    public class SpriteOverlapArea : MonoBehaviour
    {
        [SerializeField] private SpriteLayeringConfig _config;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private int _deltaOrdering;

        public int SortingOrder => _config.UnitBaseOrder + _deltaOrdering * _config.OverlayDeltaOrderMultiplier;
        
        private void Start()
        {
            _spriteRenderer.sortingOrder = SortingOrder;
        }
    }
}