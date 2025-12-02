using Gameplay.Data.Orders;
using UnityEngine;

namespace Gameplay.Units
{
    public class Order
    {
        public OrderType Type { get; }
        public Unit Actor { get; }
        public OrderTarget Target { get; }
        
        public Order(OrderType type, Unit actor, OrderTarget target)
        {
            Type = type;
            Actor = actor;
            Target = target;
        }

        public void OnProceed() => Type.OnProceed(this);
        
        public void OnUpdate() => Type.OnUpdate(this);
        
        public bool IsCarriedOut() => Type.IsCarriedOut(this);

        public void Dispose() => Type.Dispose(this);
    }

    public struct OrderTarget
    {
        public Vector2 Point { get; set; }
        public Unit Unit { get; set; }
        
        public OrderTarget(Vector2 point, Unit unit)
        {
            Point = point;
            Unit = unit;
        }
    }
}