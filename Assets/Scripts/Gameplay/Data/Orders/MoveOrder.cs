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
        
        public override async UniTask CarryOut(Order order, CancellationToken ct)
        {
            try
            {
                order.Actor.Movement.Move(order.Target.Point);
                await UniTask.WaitUntil(() => ! order.Actor.Movement.HasPath, PlayerLoopTiming.FixedUpdate, ct);
            }
            catch (OperationCanceledException e)
            {

            }
            finally
            {
                order.Actor.Movement.Stop();
            }
        }
    }
}