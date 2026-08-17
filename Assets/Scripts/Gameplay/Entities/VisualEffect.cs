using System;
using _Core;
using Gameplay.Data.Units;
using Gameplay.Vision;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Entities
{
    public class VisualEffect : Entity
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private UnitSpriteAnchor _unitSpriteAnchor;

        private IDisposable _visibilityObservable;
        private IDisposable _attachObservable;
        
        [Inject] private VisionMap VisionMap { get; set; }

        public override void OnSpawn()
        {
            _visibilityObservable = Observable.EveryUpdate()
                .Subscribe(o => UpdateVisibility());
        }

        protected override void OnDespawn()
        {
            _attachObservable?.Dispose();
            _attachObservable = null;
            _visibilityObservable?.Dispose();
            _visibilityObservable = null;
        }

        public void AttachToUnit(Unit unit)
        {
            _attachObservable = Observable.EveryUpdate()
                .Subscribe(o => FollowUnit(unit));
        }

        private void UpdateVisibility()
        {
            _spriteRenderer.enabled = VisionMap.IsPointVisibleBy(transform.position, Owner.Player);
        }

        private void FollowUnit(Unit unit)
        {
            Vector2 offset = unit.Type.SpriteMap?.GetAnchorOffset(_unitSpriteAnchor) ?? Vector2.zero;
            transform.position = unit.Position + offset;
        }
    }
}