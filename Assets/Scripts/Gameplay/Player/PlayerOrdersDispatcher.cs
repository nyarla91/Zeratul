using Gameplay.Data.Configs;
using Gameplay.Data.Orders;
using Gameplay.Units;

namespace Gameplay.Player
{
    public class PlayerOrdersDispatcher
    {
        private readonly PlayerSelection _playerSelection;
        private readonly PlayerInput _playerInput;
        private readonly OrderErrorConfig _errors;

        private bool QueueOrder => _playerInput.QueueOrder.IsHeld;

        public PlayerOrdersDispatcher(PlayerSelection playerSelection, PlayerInput playerInput, OrderErrorConfig errors)
        {
            _playerSelection = playerSelection;
            _playerInput = playerInput;
            _errors = errors;
        }

        public void IssueSmartOrderToSelection(OrderTarget target)
        {
            foreach (Unit unit in _playerSelection.SelectedUnits)
            {
                unit.Orders.IssueSmartOrder(target, QueueOrder);
            }   
        }
        
        public void IssueOrderToSelection(OrderType type, OrderTarget target)
        {
            foreach (Unit unit in _playerSelection.SelectedUnits)
            {
                unit.Orders.IssueOrder(new Order(type, unit, target), QueueOrder);
            }
        }

        public bool CanIssueWithoutTarget(OrderType orderType, out string errorMessage)
        {
            errorMessage = _errors.Generic;
            foreach (Unit selectedUnit in _playerSelection.SelectedUnits)
            {
                if (orderType.IsActorValid(selectedUnit, out errorMessage))
                    return true;
            }
            return false;
        }

        public bool CanIssueWithTarget(OrderType orderType, OrderTarget target, out string errorMessage)
        {
            if (!CanIssueWithoutTarget(orderType, out errorMessage))
                return false;
            foreach (Unit selectedUnit in _playerSelection.SelectedUnits)
            {
                if (orderType.IsTargetValid(selectedUnit, target, out errorMessage))
                    return true;
            }
            return false;
        }
    }
}