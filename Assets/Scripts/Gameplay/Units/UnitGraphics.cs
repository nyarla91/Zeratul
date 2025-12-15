using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitGraphics : UnitComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _interactionCollider;

        public void Init(UnitType unitType)
        {
            Vector2 offset = Vector2.up * unitType.Graphics.SpriteHeight;
            _spriteRenderer.transform.localPosition = offset;
            _interactionCollider.localPosition = offset;
            _interactionCollider.localScale = Vector3.one * unitType.Movement.Size;
        }
        
        private void Update()
        {
            _spriteRenderer.sprite = UnitType.Graphics.SpriteMap.GetSpriteForAngle(Composition.Movement.LookAngle);
        }
    }
}