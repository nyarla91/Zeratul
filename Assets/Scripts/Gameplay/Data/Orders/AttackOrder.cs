using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Attack Order", order = 0)]
    public class AttackOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.Unit;

        public override bool IsValidForSmartOrder(OrderTarget target) => target.Unit != null && ! target.Unit.Ownership.OwnedByPlayer;
        
        public override void OnProceed(Order order)
        {
            order.Actor.Attack.StartAttacking(order.Target.Unit);
        }

        public override void OnUpdate(Order order) { }

        public override void Dispose(Order order)
        {
            order.Actor.Attack.StopAttacking();
        }

        public override bool IsCompleted(Order order)
            => order.Target.Unit is null || order.Target.Unit == order.Actor ||
               ! order.Target.Unit.Visibility.CanBeTargetedBy(order.Actor) || order.Target.Unit.Life.HitPoints <= 0;
    }
}