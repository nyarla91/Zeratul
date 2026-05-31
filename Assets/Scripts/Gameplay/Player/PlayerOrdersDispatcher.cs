using System.Linq;
using Gameplay.Data.Configs;
using Gameplay.Data.Orders;
using Gameplay.Units;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerOrdersDispatcher
    {
        private readonly PlayerSelection _playerSelection;
        private readonly PlayerInput _playerInput;
        private readonly OrderErrorConfig _errors;

        private bool QueueOrder => _playerInput.QueueOrder.IsHeld;

        [Inject]
        public PlayerOrdersDispatcher(PlayerSelection playerSelection, PlayerInput playerInput, OrderErrorConfig errors)
        {
            _playerSelection = playerSelection;
            _playerInput = playerInput;
            _errors = errors;
        }

        public void IssueSmartOrderToSelection(OrderTarget target)
        {
            foreach (Unit unit in _playerSelection.SelectedPlayerUnits)
            {
                unit.Orders.IssueSmartOrder(target, QueueOrder);
            }   
        }
        
        public void IssueOrderToSelection(OrderType type, OrderTarget target)
        {
            foreach (Unit unit in _playerSelection.SelectedPlayerUnits)
            {
                unit.Orders.IssueOrder(new Order(type, unit, target), QueueOrder);
            }
        }

        public bool CanIssueWithoutTarget(OrderType orderType, out string errorMessage)
        {
            errorMessage = _errors.Generic;
            foreach (Unit selectedUnit in _playerSelection.SelectedPlayerUnits)
            {
                if ( ! selectedUnit.Type.AvailableOrders.Contains(orderType))
                    continue;
                if (orderType.CanBeDisplayed(selectedUnit) && orderType.IsActorValid(selectedUnit, out errorMessage))
                    return true;
            }
            return false;
        }

        public bool CanIssueWithTarget(OrderType orderType, OrderTarget target) =>
            CanIssueWithTarget(orderType, target, out _);
        
        public bool CanIssueWithTarget(OrderType orderType, OrderTarget target, out string errorMessage)
        {
            if ( ! CanIssueWithoutTarget(orderType, out errorMessage))
                return false;
            if (orderType.TargetRequirement == TargetRequirement.Unit && target.Unit == null)
            {
                errorMessage = _errors.MustBeUnit;
                return false;
            }
            foreach (Unit selectedUnit in _playerSelection.SelectedPlayerUnits)
            {
                if ( ! selectedUnit.Type.AvailableOrders.Contains(orderType))
                    continue;
                if (orderType.IsTargetValid(selectedUnit, target, out errorMessage))
                    return true;
            }
            return false;
        }
    }
}