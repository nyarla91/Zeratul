using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Move Order", order = 0)]
    public class MoveOrder : OrderType
    {
        [Space]
        [SerializeField] private float _followMinDistance;
        [SerializeField] private int _framesBetweenFollowRecalculation;
        [SerializeField] private OrderErrorConfig _errors;

        public override TargetRequirement TargetRequirement => TargetRequirement.PointOrUnit;

        public override bool IsValidForSmartOrder(OrderTarget target)
            => target.Unit == null || target.Unit.Ownership.OwnedByPlayer;
        
        public override bool IsActorValid(Unit actor, out string errorMessage)
        {
            if ( ! actor.CanMove)
            {
                errorMessage = _errors.CannotMove;
                return false;
            }
            errorMessage = null;
            return true;
        }

        public override bool IsTargetValid(Unit actor, OrderTarget target, out string errorMessage)
        {
            errorMessage = null;
            return target.Unit != actor;
        }

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            if ( ! order.Target.Unit)
            {
                order.Actor.Movement.Move(order.Target.Point);
                await UniTask.WaitUntil(() => ! order.Actor.Movement.HasPath, PlayerLoopTiming.FixedUpdate, ct);
                return;
            }

            while (order.Target.Unit)
            {
                Vector2 rawDelta = order.Target.Unit.Position - order.Actor.Position;
                Vector2 deIsoDelta = rawDelta / Isometry.Scale;
                float redundantDistance = order.Target.Unit.Type.Size / 2 + order.Actor.Type.Size / 2;
                float distance = deIsoDelta.magnitude - redundantDistance;
                
                if (distance < _followMinDistance)
                    order.Actor.Movement.Stop();
                else
                    order.Actor.Movement.Move(order.Target.Unit.Position);
                
                for (int i = 0; i < _framesBetweenFollowRecalculation; i++)
                {
                    await UniTask.WaitForFixedUpdate(ct);
                }
            }
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }
    }
}