using System;
using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitGraphics : UnitComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Init(UnitType unitType)
        {
            
        }
        
        private void Update()
        {
            _spriteRenderer.sprite = UnitType.Graphics.SpriteMap.GetSpriteForAngle(Composition.Movement.LookAngle);
        }
    }
}