using System;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitGraphics : UnitComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private void Update()
        {
            _spriteRenderer.sprite = UnitType.Graphics.SpriteMap.GetSpriteForAngle(Composition.Movement.LookAngle);
        }
    }
}