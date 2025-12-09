using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerOrdersDispatcher : MonoBehaviour
    {
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        [Inject] private PlayerInput PlayerInput { get; set; }

        private bool QueueOrder => PlayerInput.QueueOrderBinding.IsHeld;
        
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

    }
}