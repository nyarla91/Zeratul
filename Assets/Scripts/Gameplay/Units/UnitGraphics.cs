using System;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitGraphics : UnitComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _selectionCircle;

        private void Awake() => _selectionCircle.localScale = Vector3.one * UnitType.Movement.Size;

        private void Update()
        {
            _spriteRenderer.sprite = UnitType.Graphics.SpriteMap.GetSpriteForAngle(Composition.Movement.LookAngle);
        }
    }
}