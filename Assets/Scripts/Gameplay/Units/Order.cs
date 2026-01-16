using System.Threading;
using Cysharp.Threading.Tasks;
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

        public UniTask CarryOut(CancellationToken ct) => Type.CarryOut(this, ct);

        public bool CanBeIssued() => Type.CanBeIssued(this);
        
        public void Complete() => Type.Complete(this);
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