using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Attack Order", order = 0)]
    public class AttackOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.Unit;

        public override bool IsValidForSmartOrder(OrderTarget target) => target.Unit != null && ! target.Unit.Ownership.OwnedByPlayer;

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            order.Actor.Attack.StartAttacking(order.Target.Unit);
            await UniTask.WaitUntil(() => IsCompleted(order), PlayerLoopTiming.FixedUpdate, ct);
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
            order.Actor.Attack.StopAttacking();
        }

        public override bool CanBeIssued(Order order) =>
            order.Actor.Attack.IsAbleToAttack && order.Target.Unit != order.Actor;

        public bool IsCompleted(Order order)
            => order.Target.Unit is null || order.Target.Unit == order.Actor ||
               ! order.Target.Unit.Visibility.CanBeTargetedBy(order.Actor) || order.Target.Unit.Life.HitPoints <= 0;
    }
}