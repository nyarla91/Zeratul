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
        private readonly OrderErrorConfig _errors;
        private readonly GamePause _gamePause;
        private readonly ClickArea _clickArea;
        private readonly Message _message;
        
        public OrderType CurrentOrder { get; private set; }
        public OrderTarget CurrentTarget { get; private set; }
        public bool IsTargeting => CurrentOrder;

        
        [Inject]
        public PlayerOrderTargeter(PlayerInput input, PlayerMouseTargeting mouseTargeting, PlayerSelection selection,
            PlayerOrdersDispatcher ordersDispatcher, OrderErrorConfig errors, GamePause gamePause, ClickArea clickArea,
            Message message)
        {
            _input = input;
            _mouseTargeting = mouseTargeting;
            _selection = selection;
            _ordersDispatcher = ordersDispatcher;
            _errors = errors;
            _gamePause = gamePause;
            _clickArea = clickArea;
            _message = message;
            
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
                _clickArea.LeftClicked += DispatchOrderWithTarget;
                _clickArea.RightClicked += CancelTargeting;
            }
            else
            {
                _clickArea.LeftClicked -= DispatchOrderWithTarget;
                _clickArea.RightClicked -= CancelTargeting;
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
                _message.Show(errorMessage, MessageType.Error);
                return;
            }
            _ordersDispatcher.IssueOrderToSelection(CurrentOrder, CurrentTarget);
            if ( ! _input.QueueOrder.IsHeld)
                CancelTargeting();
        }

        private void UpdateCurrentTarget()
        {
            if (_gamePause.IsPaused)
            {
                DispatchOrderWithTarget();
            }
            CurrentTarget = _mouseTargeting.GetTargetForRequirement(CurrentOrder?.TargetRequirement ?? TargetRequirement.None);
        }
    }
}