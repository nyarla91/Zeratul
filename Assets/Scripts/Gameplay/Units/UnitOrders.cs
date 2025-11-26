using System.Collections.Generic;
using Gameplay.Pathfinding;
using Gameplay.Units.Orders;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitOrders : UnitComponent
    {
        private readonly List<Order> _pendingOrders = new();
        private Order _currentOrder;
        
        [Inject] public NodeMap NodeMap { get; private set; }

        public void IssueSmartOrder(Vector2 point,  bool queue)
        {
            if ( ! NodeMap.IsPointPassable(point))
                return;
            
            Order newOrder = new MoveToPointOrder(Composition, point);

            if (queue)
            {
                _pendingOrders.Add(newOrder);
            }
            else
            {
                ProceedToOrder(newOrder);
                _pendingOrders.Clear();
            }
        }

        private void FixedUpdate()
        {
            if (_currentOrder == null)
            {
                TryProceedToNextOrder();
            }
            else if (_currentOrder.IsCarriedOut())
            {
                if (!TryProceedToNextOrder())
                {
                    _currentOrder.Dispose();
                    _currentOrder = null;
                }
            }
            _currentOrder?.OnUpdate(Time.fixedDeltaTime);
        }

        private bool TryProceedToNextOrder()
        {
            if (_pendingOrders.Count == 0)
                return false;
            ProceedToOrder(_pendingOrders[0]);
            _pendingOrders.RemoveAt(0);
            return true;
        }

        private void ProceedToOrder(Order order)
        {
            _currentOrder?.Dispose();
            _currentOrder = order;
            _currentOrder.OnProceed();
        }
    }
}