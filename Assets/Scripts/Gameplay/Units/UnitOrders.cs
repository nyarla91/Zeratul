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

        private static readonly Func<KeyControl>[] Keys =
        {
            () => Keyboard.current.digit1Key,
            () => Keyboard.current.digit2Key,
            () => Keyboard.current.digit3Key,
            () => Keyboard.current.digit4Key,
            () => Keyboard.current.digit5Key,
            () => Keyboard.current.digit6Key,
            () => Keyboard.current.digit7Key,
            () => Keyboard.current.digit8Key,
            () => Keyboard.current.digit9Key,
            () => Keyboard.current.digit0Key,
        };
        
        [Inject] public NodeMap NodeMap { get; private set; }

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

        private void Update()
        {
            for (int i = 0; i < Keys.Length && i < UnitType.AvailableOrders.Length; i++)
            {
                if ( ! Keys[i].Invoke().wasPressedThisFrame)
                    continue;
                
                OrderType orderType = UnitType.AvailableOrders[i];
                bool queue = Keyboard.current.leftShiftKey.isPressed;
                OrderTarget target = new OrderTarget();
                
                if (orderType.TargetRequirement == TargetRequirement.None)
                {
                    IssueOrder(new Order(orderType, Composition, target), queue);
                    return;
                }
                
                target.Point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if (orderType.TargetRequirement == TargetRequirement.Point)
                {
                    IssueOrder(new Order(orderType, Composition, target), queue);
                    return;
                }
                
                target.Unit = Physics2D.OverlapPointAll(target.Point).Select(col => col?.GetComponent<Unit>())
                    .ClearNull()?[0];
                if (orderType.TargetRequirement == TargetRequirement.PointOrUnit)
                {
                    IssueOrder(new Order(orderType, Composition, target), queue);
                    return;
                }
                if (target.Unit is null)
                    return;
                IssueOrder(new Order(orderType, Composition, target), queue);
            }
        }
    }
}