using System;
using Extentions.Pause;
using Gameplay.Data.Configs;
using Gameplay.Data.Orders;
using Gameplay.UI;
using Gameplay.Units;
using UniRx;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerOrderTargeter
    {
        private readonly PlayerInput _input;
        private readonly PlayerMouseTargeting _mouseTargeting;
        private readonly PlayerSelection _selection;
        private readonly PlayerOrdersDispatcher _ordersDispatcher;
        
        public OrderType CurrentOrder { get; private set; }
        public OrderTarget CurrentTarget { get; private set; }
        public bool IsTargeting => CurrentOrder;

        [Inject] private GamePause GamePause { get; set; }
        [Inject] private ClickArea ClickArea { get; set; }
        [Inject] private OrderErrorMessage OrderErrorMessage { get; set; }
        [Inject] private OrderErrorConfig Errors { get; set; }
        
        public PlayerOrderTargeter(PlayerInput input, PlayerMouseTargeting mouseTargeting, PlayerSelection selection, PlayerOrdersDispatcher ordersDispatcher)
        {
            _input = input;
            _mouseTargeting = mouseTargeting;
            _selection = selection;
            _ordersDispatcher = ordersDispatcher;
            Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateCurrentTarget());
            
            this.ObserveEveryValueChanged(t => t.IsTargeting)
                .Skip(1)
                .Subscribe(UpdateSubscriptions);
        }

        private void UpdateSubscriptions(bool isTargeting)
        {
            if (isTargeting)
            {
                ClickArea.LeftClicked += DispatchOrderWithTarget;
                ClickArea.RightClicked += CancelTargeting;
            }
            else
            {
                ClickArea.LeftClicked -= DispatchOrderWithTarget;
                ClickArea.RightClicked -= CancelTargeting;
            }
        }

        public void StartTargeting(OrderType order)
        {
            if (order.TargetRequirement == TargetRequirement.None)
                throw new ArgumentException($"Target is not required");
            CurrentOrder = order;
            UpdateCurrentTarget();
        }

        public void CancelTargeting()
        {
            CurrentOrder = null;
        }

        private void DispatchOrderWithTarget()
        {
            if ( ! IsTargeting)
                return;
            if ( ! _ordersDispatcher.CanIssueWithTarget(CurrentOrder, CurrentTarget, out string errorMessage))
            {
                OrderErrorMessage.Show(errorMessage);
                return;
            }
            _ordersDispatcher.IssueOrderToSelection(CurrentOrder, CurrentTarget);
            if ( ! _input.QueueOrder.IsHeld)
                CancelTargeting();
        }

        private void UpdateCurrentTarget()
        {
            if (GamePause.IsPaused)
            {
                DispatchOrderWithTarget();
            }
            CurrentTarget = _mouseTargeting.GetTargetForRequirement(CurrentOrder?.TargetRequirement ?? TargetRequirement.None);
        }
    }
}