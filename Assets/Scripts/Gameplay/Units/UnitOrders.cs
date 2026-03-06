using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay.Data;
using Gameplay.Data.Orders;
using Gameplay.Pathfinding;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitOrders : UnitComponentMono
    {
        private readonly Queue<Order> _pendingOrders = new();

        private CancellationTokenSource _currentOrderCts;
        private UniTask _currentOrderTask;
        
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
                IssueOrder(new Order(orderType, Unit, target), queue);
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
            _pendingOrders.Enqueue(order);
        }

        public void CompleteCurrentOrder()
        {
            _currentOrderCts?.Cancel();
            CurrentOrder = null;
        }

        private void FixedUpdate()
        {
            if (TacticalPause.IsPaused)
                return;
            
            if (CurrentOrder != null && (CurrentOrder.MustBeCanceled() || _currentOrderTask.GetAwaiter().IsCompleted))
            {
                CompleteCurrentOrder();
            }

            if ( ! _currentOrderTask.GetAwaiter().IsCompleted)
                return;
            if (CurrentOrder != null || _pendingOrders.Count <= 0)
                return;
            CurrentOrder = _pendingOrders.Dequeue();
                
            _currentOrderCts = new CancellationTokenSource();
            _currentOrderTask = CurrentOrder.CarryOut(_currentOrderCts.Token);
        }

        private void OnDestroy()
        {
            CurrentOrder =  null;
            _pendingOrders.Clear();
        }
    }
}