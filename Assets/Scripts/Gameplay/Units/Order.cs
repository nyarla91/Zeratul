using System;
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

        public bool MustBeCanceled() => Type.MustBeCanceled(this);
    }

    public struct OrderTarget
    {
        private Vector2 _point;
        public Vector2 Point
        {
            readonly get
            {
                /*if (_point == default)
                    throw new InvalidOperationException("Do not access OrderTarget.Point if it's default");*/
                return _point;
            }
            set => _point = value;
        }

        public Unit Unit { get; }
        
        public OrderTarget(Vector2 point, Unit unit)
        {
            _point = point;
            Unit = unit;
        }
    }
}