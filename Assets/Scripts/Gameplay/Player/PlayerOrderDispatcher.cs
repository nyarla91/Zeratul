using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Player
{
    public class PlayerOrderDispatcher : MonoBehaviour
    {
        [SerializeField] private PlayerSelection _playerSelection;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _clickEventIndex;

        public void IssueOrderToSelection(OrderType orderType, OrderTarget target)
        {
            foreach (Unit unit in _playerSelection.SelectedUnits)
            {
                unit.Orders.IssueOrder(new Order(orderType, unit, target), false);
            }
        }
    }
}