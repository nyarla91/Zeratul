using Gameplay.Data.Orders;
using UnityEngine;

namespace Gameplay.Units
{
    public class Order
    {
        public OrderType Type { get; }
        public Unit Actor { get; }
        public Vector2 TargetPoint { get; }
        public Unit TargetUnit { get; }
        
        public Order(OrderType type, Unit actor, Vector2 targetPoint, Unit targetUnit)
        {
            Type = type;
            Actor = actor;
            TargetPoint = targetPoint;
            TargetUnit = targetUnit;
        }

        public void OnProceed() => Type.OnProceed(this);
        
        public void OnUpdate() => Type.OnUpdate(this);
        
        public bool IsCarriedOut() => Type.IsCarriedOut(this);

        public void Dispose() => Type.Dispose(this);
    }
}