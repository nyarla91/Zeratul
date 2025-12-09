using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Orders;
using Gameplay.Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Zenject;

namespace Gameplay.Units
{
    public class UnitOrders : UnitComponent
    {
        private readonly List<Order> _pendingOrders = new();
        private Order _currentOrder;

        public bool IsIdle => _currentOrder == null;
        
        [Inject] public NodeMap NodeMap { get; private set; }

        public void IssueSmartOrder(OrderTarget target, bool queue)
        {
            foreach (OrderType orderType in UnitType.AvailableOrders)
            {
                if ( ! orderType.IsValidForSmartOrder(target))
                    continue;
                IssueOrder(new Order(orderType, Composition, target), queue);
                break;
            }
        }
        
        public void IssueOrder(Order order,  bool queue)
        {
            if ( ! UnitType.AvailableOrders.Contains(order.Type))
                return;
            
            if (queue)
            {
                _pendingOrders.Add(order);
            }
            else
            {
                ProceedToOrder(order);
                _pendingOrders.Clear();
            }
        }

        private void FixedUpdate()
        {
            if (IsIdle)
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
            _currentOrder?.OnUpdate();
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