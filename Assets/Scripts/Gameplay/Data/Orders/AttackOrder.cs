using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Attack Order", order = 0)]
    public class AttackOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.PointOrUnit;

        public override bool IsValidForSmartOrder(OrderTarget target) => target.Unit != null && ! target.Unit.Ownership.OwnedByPlayer;

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            if ( ! order.Actor.CanAttack)
                return;
            
            UnitAttack actorAttack = order.Actor.Attack;
            if (order.Target.Unit)
            {
                actorAttack.StartAttacking(order.Target.Unit);
                await UniTask.WaitUntil(() => ! order.Actor.Attack.IsAttacking, PlayerLoopTiming.FixedUpdate, ct);
                return;
            }
            
            if ( ! order.Actor.CanMove)
                return;
            
            UnitMovement actorMovement = order.Actor.Movement;
            do
            {
                if (!actorAttack.IsAttacking && actorAttack.ClosestTarget)
                    actorAttack.StartAttacking(actorAttack.ClosestTarget);

                await UniTask.WaitUntil(() => !actorAttack.IsAttacking, PlayerLoopTiming.FixedUpdate, ct);

                if ( ! actorMovement.HasPath)
                    actorMovement.Move(order.Target.Point);
                
                await UniTask.WaitForFixedUpdate(ct);
            }
            while (actorMovement.HasPath || actorAttack.IsAttacking);
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement?.Stop();
            order.Actor.Attack?.StopAttacking();
        }

        public override bool CanBeIssued(Order order)
        {
            return order.Actor.CanAttack
                   && (order.Target.Unit || order.Actor.CanMove)
                   && (!order.Target.Unit || order.Actor.Attack.CanAttackUnit(order.Target.Unit));
        }
    }
}