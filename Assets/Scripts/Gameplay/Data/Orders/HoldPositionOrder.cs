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
        
        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            order.Actor.Movement.HoldPosition();
            await UniTask.Never(ct);
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement.StopHoldingPosition();
        }

        public override bool CanBeIssued(Order order) => order.Actor.CanMove;
    }
}