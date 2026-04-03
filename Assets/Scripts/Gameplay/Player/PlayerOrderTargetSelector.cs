using System;
using Extentions.Pause;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerOrderTargetSelector
    {
        private readonly PlayerMouseTargeting _mouseTargeting;
        
        public OrderType CurrentOrder { get; private set; }
        public OrderTarget CurrentTarget { get; private set; }
        public bool IsTargeting => CurrentOrder;

        private OrderTarget EstimatedPointOrUnitTarget => new(_mouseTargeting.Point, _mouseTargeting.Unit);

        [Inject] private GamePause GamePause { get; set; }
        
        public PlayerOrderTargetSelector(PlayerMouseTargeting mouseTargeting)
        {
            _mouseTargeting = mouseTargeting;
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
                TargetRequirement.Point => new OrderTarget(_mouseTargeting.Point, null),
                TargetRequirement.Unit => new OrderTarget(default, _mouseTargeting.Unit),
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