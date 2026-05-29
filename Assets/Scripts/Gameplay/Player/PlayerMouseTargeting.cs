using System;
using System.Linq;
using Extentions;
using Extentions.Pause;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Player
{
    public class PlayerMouseTargeting
    {
        private readonly Camera _mainCamera;
        private readonly LayerMask _unitsMask;

        public Unit Unit { get; private set; }
        public Vector2 Point { get; private set; }
        
        [Inject] public GamePause GamePause { get; }
        
        public PlayerMouseTargeting(Camera mainCamera, LayerMask unitsMask)
        {
            _mainCamera = mainCamera;
            _unitsMask = unitsMask;

            Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateTargets());
        }

        public OrderTarget GetTargetForRequirement(TargetRequirement requirement)
        {
            return requirement switch
            {
                TargetRequirement.None => default,
                TargetRequirement.Point => OrderTarget.FromPoint(Point),
                TargetRequirement.Unit => OrderTarget.FromUnit(Unit),
                TargetRequirement.PointOrUnit => new OrderTarget(Point, Unit),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void UpdateTargets()
        {
            if (GamePause.IsPaused)
                return;
            
            Point = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            Collider2D[] overlap = Physics2D.OverlapPointAll(Point, _unitsMask);
            Unit[] units = overlap.Select(x => x.transform.GetComponentInParent<Unit>()).ClearNull();
            units = units.Where(u => u.IsInteractable && u.IsVisibleToPlayer).ToArray();

            Unit = units.Length == 0 ? null : units.MinElement(u => Isometry.Distance(Point, u.InteractionPosition));
        }
    }
}