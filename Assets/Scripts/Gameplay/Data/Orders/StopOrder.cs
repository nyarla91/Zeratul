using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Stop Order", order = 0)]
    public class StopOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.None;

        protected override UniTask CarryOutBody(Order order, CancellationToken ct) => UniTask.CompletedTask;
        
        protected override void Dispose(Order order) { }
    }
}