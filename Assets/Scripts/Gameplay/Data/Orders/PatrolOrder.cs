using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Patrol Order", order = 0)]
    public class PatrolOrder : OrderType
    {
        
        public override TargetRequirement TargetRequirement => TargetRequirement.Point;
        
        public override async UniTask CarryOut(Order order, CancellationToken ct)
        {
            try
            {
                Vector2 originalPoint = order.Actor.transform.position;
                bool moveBackwards = false;

                while (true)
                {
                    Vector2 nextPoint = moveBackwards ? order.Target.Point : originalPoint;
                    order.Actor.Movement.Move(nextPoint);

                    await UniTask.WaitUntil(() => ! order.Actor.Movement.HasPath, PlayerLoopTiming.FixedUpdate, ct);
                    moveBackwards = !moveBackwards;
                }
                
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