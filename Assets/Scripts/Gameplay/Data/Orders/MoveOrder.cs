using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Move Order", order = 0)]
    public class MoveOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.Point;

        public override bool IsValidForSmartOrder(OrderTarget target) => target.Unit == null;

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            order.Actor.Movement.Move(order.Target.Point);
            await UniTask.WaitUntil(() => ! order.Actor.Movement.HasPath, PlayerLoopTiming.FixedUpdate, ct);
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }
    }
}