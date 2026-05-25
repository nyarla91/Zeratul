using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Orders;
using Save.Data.Units;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitOrders : UnitComponent
    {
        protected override string LoadKey => UnitOrdersSaveSystem.LoadKey;

        private readonly GameDataRegistry _gameDataRegistry;
        private readonly IGetUnitByIdService _getUnitByIdService;
        
        private readonly Queue<Order> _ordersQueue = new();
        private CancellationTokenSource _currentOrderCts;
        private UniTask _currentOrderTask;
        
        public Order CurrentOrder { get; private set; }

        public Order[] OrdersQueue => _ordersQueue.ToArray();
        public bool IsIdle => CurrentOrder == null;

        public event Action<Order> CurrentOrderUpdated;
        
        public UnitOrders(Unit unit, IPauseReadonly tacticalPause, GameDataRegistry gameDataRegistry, IGetUnitByIdService getUnitByIdService) : base(unit)
        {
            _gameDataRegistry = gameDataRegistry;
            _getUnitByIdService = getUnitByIdService;
            
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Where(_ => CurrentOrder != null)
                .Where(_ => CurrentOrder.MustBeCanceled() || _currentOrderTask.GetAwaiter().IsCompleted)
                .Subscribe(_ => CompleteCurrentOrder());
            
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Where(_ => _currentOrderTask.GetAwaiter().IsCompleted)
                .Where(_ => CurrentOrder == null)
                .Where(_ => _ordersQueue.Count > 0)
                .Subscribe(_ => TryCarryOutNextOrder());

            Unit.Alliance.OwnerUpdated += _ => ClearAllOrders();
        }

        public override IUnitSaveSystem Save()
        {
            OrderSaveData[] queue = OrdersQueue
                .Prepend(CurrentOrder)
                .Where(o => o != null)
                .Select(OrderToSaveData)
                .ToArray();
            
            
            return new UnitOrdersSaveSystem(queue);
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitOrdersSaveSystem system = GetSaveSystem<UnitOrdersSaveSystem>(saveData);
            foreach (OrderSaveData orderSaveData in system.queue)
            {
                Order order = OrderFromSaveData(orderSaveData);
                IssueOrder(order, true);
            }
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

            if ( ! queue)
            {
                ClearAllOrders();
            }
            _ordersQueue.Enqueue(order);
        }

        public void CompleteCurrentOrder()
        {
            _currentOrderCts?.Cancel();
            CurrentOrder = null;
            CurrentOrderUpdated?.Invoke(CurrentOrder);
        }

        private bool TryCarryOutNextOrder()
        {
            if (CurrentOrder != null)
                return false;
            CurrentOrder = _ordersQueue.Dequeue();
            CurrentOrderUpdated?.Invoke(CurrentOrder);
            _currentOrderCts = new CancellationTokenSource();
            _currentOrderTask = CurrentOrder.CarryOut(_currentOrderCts.Token);
            return true;
        }

        private void ClearAllOrders()
        {
            CompleteCurrentOrder();
            CurrentOrderUpdated?.Invoke(CurrentOrder);
            _ordersQueue.Clear();
        }

        private OrderSaveData OrderToSaveData(Order order)
        {
            string orderType = order.Type.name;
            int targetUnit = order.Target.Unit?.Id ?? -1;
            SerializableVector2 targetPoint = SerializableVector2.FromVector2(order.Target.Point);
            return new OrderSaveData(orderType, targetUnit, targetPoint);
        }

        private Order OrderFromSaveData(OrderSaveData saveData)
        {
            OrderType orderType = _gameDataRegistry.Get<OrderType>(saveData.orderType);
            Unit targetUnit = _getUnitByIdService.GetUnitById(saveData.targetUnit);
            Debug.Log($"{saveData.targetUnit} {targetUnit}");
            Vector2 targetPoint = saveData.targetPoint.ToVector2();
            return new Order(orderType, Unit, new OrderTarget(targetPoint, targetUnit));
        }
    }
}