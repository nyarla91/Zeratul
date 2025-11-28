using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Orders;
using Gameplay.Pathfinding;
using Source.Extentions;
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
            for (int i = 0; i < Keys.Length && i < Type.AvailableOrders.Length; i++)
            {
                if ( ! Keys[i].Invoke().wasPressedThisFrame)
                    continue;
                
                OrderType orderType = Type.AvailableOrders[i];
                bool queue = Keyboard.current.leftShiftKey.isPressed;

                if (orderType.TargetRequirement == TargetRequirement.None)
                {
                    IssueOrder(new Order(orderType, Composition, default, null), queue);
                    return;
                }
                
                Vector2 targetPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                if (orderType.TargetRequirement == TargetRequirement.Point)
                {
                    IssueOrder(new Order(orderType, Composition, targetPoint, null), queue);
                    return;
                }
                
                Unit targetUnit = Physics2D.OverlapPointAll(targetPoint).Select(col => col?.GetComponent<Unit>())
                    .ClearNull()?[0];
                if (orderType.TargetRequirement == TargetRequirement.PointOrUnit)
                {
                    IssueOrder(new Order(orderType, Composition, targetPoint, targetUnit), queue);
                    return;
                }
                if (targetUnit is null)
                    return;
                IssueOrder(new Order(orderType, Composition, default, targetUnit), queue);
            }
        }
    }
}