using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Gameplay.Data.Orders;
using Gameplay.Pathfinding;
using Zenject;

namespace Gameplay.Units
{
    public class UnitOrders : UnitComponent
    {
        private readonly List<Order> _pendingOrders = new();

        public Order CurrentOrder { get; private set; }

        public Order[] PendingOrders => _pendingOrders.ToArray();

        public bool IsIdle => CurrentOrder == null;
        
        [Inject] private NodeMap NodeMap { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }

        public void Init(UnitType unitType)
        {
            
        }
        
        public void IssueSmartOrder(OrderTarget target, bool queue)
        {
            foreach (OrderType orderType in UnitType.AvailableOrders)
            {
                if ( ! orderType || ! orderType.IsValidForSmartOrder(target))
                    continue;
                IssueOrder(new Order(orderType, Composition, target), queue);
                break;
            }
        }
        
        public void IssueOrder(Order order,  bool queue)
        {
            if ( ! UnitType.AvailableOrders.Contains(order.Type))
                return;
            if ( ! order.CanBeIssued())
                return;

            if (!queue)
            {
                CompleteCurrentOrder();
                _pendingOrders.Clear();
            }
            _pendingOrders.Add(order);
        }

        public void CompleteCurrentOrder()
        {
            CurrentOrder?.Dispose();
            CurrentOrder = null;
        }

        private void FixedUpdate()
        {
            if (TacticalPause.IsPaused)
                return;
            
            if (CurrentOrder != null && CurrentOrder.IsCompleted())
            {
                CompleteCurrentOrder();
            }
            if (CurrentOrder == null && _pendingOrders.Count > 0)
            {
                CurrentOrder = _pendingOrders[0];
                _pendingOrders.RemoveAt(0);
                CurrentOrder.OnProceed();
            }
            CurrentOrder?.OnUpdate();
        }

        private void OnDestroy()
        {
            CurrentOrder =  null;
            _pendingOrders.Clear();
        }
    }
}