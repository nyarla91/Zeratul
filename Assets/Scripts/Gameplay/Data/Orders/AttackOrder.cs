using System;
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
        
        public override async UniTask CarryOut(Order order, CancellationToken ct)
        {
            try
            {
                order.Actor.Attack.StartAttacking(order.Target.Unit);
                await UniTask.WaitUntil(() => IsCompleted(order), PlayerLoopTiming.FixedUpdate, ct);
            }
            catch (OperationCanceledException e)
            {
                
            }
            finally
            {
                order.Actor.Attack.StopAttacking();
            }
        }

        public override bool CanBeIssued(Order order) =>
            order.Actor.Attack.IsAbleToAttack && order.Target.Unit != order.Actor;

        public bool IsCompleted(Order order)
            => order.Target.Unit is null || order.Target.Unit == order.Actor ||
               ! order.Target.Unit.Visibility.CanBeTargetedBy(order.Actor) || order.Target.Unit.Life.HitPoints <= 0;
    }
}