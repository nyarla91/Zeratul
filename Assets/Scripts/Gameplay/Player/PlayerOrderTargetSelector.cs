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
    public class PlayerOrderTargetSelector
    {
        private readonly LayerMask _unitsMask;

        private Vector2 EstimatedPointTarget => Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        public OrderType CurrentOrder { get; private set; }
        public OrderTarget CurrentTarget { get; private set; }
        public bool IsTargeting => CurrentOrder;
        
        private Unit EstimatedUnitTarget
        {
            get
            {
                Collider2D[] overlap = Physics2D.OverlapPointAll(EstimatedPointTarget, _unitsMask); 
                Unit[] units = overlap.Select(x => x.transform.GetComponentInParent<Unit>()).NoNull();
                if (units.Length == 0)
                    return null;
                Unit unit = units[0];
                if ( ! unit.Visibility.IsVisibleToPlayer)
                    return null;
                return unit;
            }
        }

        private OrderTarget EstimatedPointOrUnitTarget => new(EstimatedPointTarget, EstimatedUnitTarget);
        
        [Inject] private GamePause GamePause { get; set; }
        
        public PlayerOrderTargetSelector(LayerMask unitsMask)
        {
            _unitsMask = unitsMask;
            
            Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateCurrentTarget());
        }
        
        public void StartTargeting(OrderType order)
        {
            if (order.TargetRequirement == TargetRequirement.None)
                throw new ArgumentException($"Target is not required");
            if (CurrentOrder)
                return;
            CurrentOrder = order;
            UpdateCurrentTarget();
        }

        public OrderTarget FinishTargeting()
        {
            CurrentOrder = null;
            return CurrentTarget;
        }

        public OrderTarget GetTargetForRequirement(TargetRequirement requirement)
        {
            return requirement switch
            {
                TargetRequirement.None => default,
                TargetRequirement.Point => new OrderTarget(EstimatedPointTarget, null),
                TargetRequirement.Unit => new OrderTarget(default, EstimatedUnitTarget),
                TargetRequirement.PointOrUnit => EstimatedPointOrUnitTarget,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void UpdateCurrentTarget()
        {
            if (GamePause.IsPaused)
            {
                FinishTargeting();
            }
            CurrentTarget = GetTargetForRequirement(CurrentOrder?.TargetRequirement ?? TargetRequirement.None);
        }
    }
}