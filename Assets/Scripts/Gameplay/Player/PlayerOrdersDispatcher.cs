using Gameplay.Data.Configs;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerOrdersDispatcher : MonoBehaviour
    {
        [SerializeField] private OrderErrorConfig _errors;
        
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        [Inject] private PlayerInput PlayerInput { get; set; }

        private bool QueueOrder => PlayerInput.QueueOrder.IsHeld;
        
        public void IssueSmartOrderToSelection(OrderTarget target)
        {
            foreach (Unit unit in PlayerSelection.SelectedUnits)
            {
                unit.Orders.IssueSmartOrder(target, QueueOrder);
            }   
        }
        
        public void IssueOrderToSelection(OrderType type, OrderTarget target)
        {
            foreach (Unit unit in PlayerSelection.SelectedUnits)
            {
                unit.Orders.IssueOrder(new Order(type, unit, target), QueueOrder);
            }
        }

        public bool CanIssueWithoutTarget(OrderType orderType, out string errorMessage)
        {
            errorMessage = _errors.Generic;
            foreach (Unit selectedUnit in PlayerSelection.SelectedUnits)
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
            foreach (Unit selectedUnit in PlayerSelection.SelectedUnits)
            {
                if (orderType.IsTargetValid(selectedUnit, target, out errorMessage))
                    return true;
            }
            return false;
        }
    }
}