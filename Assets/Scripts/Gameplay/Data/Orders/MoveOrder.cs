using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Move Order", order = 0)]
    public class MoveOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.Point;

        public override bool IsValidForSmartOrder(OrderTarget target) => target.Unit == null;

        public override void OnProceed(Order order)
        {
            order.Actor.Movement.Move(order.Target.Point);
        }

        public override void OnUpdate(Order order) { }

        public override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }

        public override bool IsCompleted(Order order) => ! order.Actor.Movement.HasPath;
    }
}