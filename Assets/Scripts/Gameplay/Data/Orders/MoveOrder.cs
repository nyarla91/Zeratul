using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Move Order", order = 0)]
    public class MoveOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.Point;
        
        public override void OnProceed(Order order)
        {
            order.Actor.Movement.Move(order.TargetPoint);
        }

        public override void OnUpdate(Order order) { }

        public override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }

        public override bool IsCarriedOut(Order order) => ! order.Actor.Movement.HasPath;
    }
}