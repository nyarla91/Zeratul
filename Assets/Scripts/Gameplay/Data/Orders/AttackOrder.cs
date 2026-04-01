using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Attack Order", order = 0)]
    public class AttackOrder : OrderType
    {
        [Space]
        [SerializeField] private OrderErrorConfig _errors;
        
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

        public override bool IsActorValid(Unit actor, out string errorMessage)
        {
            if ( ! actor.CanAttack)
            {
                errorMessage = _errors.CannotAttack;
                return false;
            }
            errorMessage = null;
            return true;
        }

        public override bool IsTargetValid(Unit actor, OrderTarget target, out string errorMessage)
        {
            errorMessage = _errors.TargetInvalid;
            if ( ! actor.CanAttack)
                return false;
            if ( ! target.Unit && ! actor.CanMove)
            {
                errorMessage = _errors.CannotMove;
                return false;
            }
            if (target.Unit && ! actor.Attack.CanAttackUnit(target.Unit, out errorMessage))
            {
                return false;
            }
            return true;
        }
    }
}