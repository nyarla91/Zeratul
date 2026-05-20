using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Passive Order", order = 0)]
    public class PassiveOrder : OrderType
    {
        [SerializeField] private OrderErrorConfig _errors;
        
        public override TargetRequirement TargetRequirement => TargetRequirement.None;

        protected override UniTask CarryOutBody(Order order, CancellationToken ct) => UniTask.CompletedTask;

        public override bool IsActorValid(Unit actor, out string errorMessage)
        {
            errorMessage = _errors.Passive;
            return false;
        }

        protected override void Dispose(Order order) { }
    }
}