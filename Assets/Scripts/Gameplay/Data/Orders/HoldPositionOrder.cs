using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Hold Position Order", order = 0)]
    public class HoldPositionOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.None;

        public override bool IsActorValid(Unit actor, out string errorMessage)
        {
            errorMessage = null;
            return actor.CanMove;
        }

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            order.Actor.Movement.HoldPosition();
            while (true)
            {
                await UniTask.WaitForFixedUpdate(ct);
            }
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement.StopHoldingPosition();
            order.Actor.Attack.StopAttacking();
        }
    }
}