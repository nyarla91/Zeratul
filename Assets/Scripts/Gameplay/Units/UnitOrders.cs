using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
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

        public Order CurrentOrder { get; private set; }
        
        public bool IsIdle => CurrentOrder == null;
        
        [Inject] public NodeMap NodeMap { get; private set; }

        public void Init(UnitType unitType)
        {
            
        }
        
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
            if ( ! order.CanBeIssued())
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

        public void CompleteCurrentOrder()
        {
            if (TryProceedToNextOrder())
                return;
            CurrentOrder.Dispose();
            CurrentOrder = null;
        }

        private void FixedUpdate()
        {
            if (IsIdle)
            {
                TryProceedToNextOrder();
            }
            else if (CurrentOrder.IsCompleted())
            {
                CompleteCurrentOrder();
            }
            CurrentOrder?.OnUpdate();
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
            CurrentOrder?.Dispose();
            CurrentOrder = order;
            CurrentOrder.OnProceed();
        }

        private void OnDestroy()
        {
            CurrentOrder =  null;
            _pendingOrders.Clear();
        }
    }
}