using System;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitAnimationView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Start()
        {
            _spriteRenderer.transform.localPosition = _unit.Type.Graphics.SpriteHeight * Vector2.up;
        }

        private void Update()
        {
            if (!_unit.Visibility.IsVisibleToPlayer)
            {
                _spriteRenderer.sprite = null;
                return;
            }
            _spriteRenderer.sprite = _unit.Type.Graphics.SpriteMap.GetSpriteForAngle(_unit.Movement.LookAngle);
        }
    }
}