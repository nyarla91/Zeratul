using System;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitShadowView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            transform.localScale = _unit.Type.Graphics.ShadowSize * Vector3.one;
        }

        private void Update()
        {
            _spriteRenderer.color = _unit.Visibility.IsVisibleToPlayer ? Color.white : Color.clear;
        }
    }
}