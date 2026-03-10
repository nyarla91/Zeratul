using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Extentions.Pause;
using Gameplay.Data.Orders;
using UniRx;

namespace Gameplay.Units
{
    public class UnitOrders : UnitComponent
    {
        private readonly Queue<Order> _pendingOrders = new();

        private CancellationTokenSource _currentOrderCts;
        private UniTask _currentOrderTask;
        
        public Order CurrentOrder { get; private set; }

        public Order[] PendingOrders => _pendingOrders.ToArray();

        public bool IsIdle => CurrentOrder == null;
        
        public UnitOrders(Unit unit, IPauseReadonly tacticalPause) : base(unit)
        {
            IObservable<long> unpausedFixedUpdate = Observable.EveryFixedUpdate().Where(_ => tacticalPause.IsUnpaused);

            unpausedFixedUpdate
                .Where(_ => CurrentOrder != null)
                .Where(_ => CurrentOrder.MustBeCanceled() || _currentOrderTask.GetAwaiter().IsCompleted)
                .Subscribe(_ => CompleteCurrentOrder());
            
            unpausedFixedUpdate
                .Where(_ => _currentOrderTask.GetAwaiter().IsCompleted)
                .Where(_ => CurrentOrder == null)
                .Where(_ => _pendingOrders.Count > 0)
                .Subscribe(_ => TryCarryOutNextOrder());

            Unit.Killed += ClearAllOrders;
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

        private bool TryCarryOutNextOrder()
        {
            if (CurrentOrder != null)
                return false;
            CurrentOrder = _pendingOrders.Dequeue();
            _currentOrderCts = new CancellationTokenSource();
            _currentOrderTask = CurrentOrder.CarryOut(_currentOrderCts.Token);
            return true;
        }

        private void ClearAllOrders()
        {
            CurrentOrder =  null;
            _pendingOrders.Clear();
        }
    }
}