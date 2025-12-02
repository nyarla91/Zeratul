using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.UnitManagement
{
    public class UnitOrdersDispatcher : MonoBehaviour
    {
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        [Inject] private PlayerInput PlayerInput { get; set; }

        public void IssueOrderToSelection(OrderType type, OrderTarget target)
        {
            foreach (Unit unit in PlayerSelection.SelectedUnits)
            {
                unit.Orders.IssueOrder(new Order(type, unit, target), PlayerInput.QueueOrderBinding.IsHeld);
            }
        }
    }
}