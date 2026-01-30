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
        [SerializeField] private float _followMinDistance;
        [SerializeField] private int _framesBetweenFollowRecalculation;
        
        public override TargetRequirement TargetRequirement => TargetRequirement.PointOrUnit;

        public override bool IsValidForSmartOrder(OrderTarget target)
            => target.Unit == null || target.Unit.Ownership.OwnedByPlayer;

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
                Vector2 rawDelta = order.Target.Unit.transform.position - order.Actor.transform.position;
                Vector2 deIsoDelta = rawDelta / Isometry.Scale;
                float redundantDistance = order.Target.Unit.Type.Size / 2 + order.Actor.Type.Size / 2;
                float distance = deIsoDelta.magnitude - redundantDistance;
                
                if (distance < _followMinDistance)
                    order.Actor.Movement.Stop();
                else
                    order.Actor.Movement.Move(order.Target.Unit.transform.position);
                
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

        public override bool CanBeIssued(Order order)
        {
            return order.Target.Unit != order.Actor;
        }
    }
}