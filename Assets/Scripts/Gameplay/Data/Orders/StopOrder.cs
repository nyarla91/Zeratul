using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Stop Order", order = 0)]
    public class StopOrder : OrderType
    {
        public override TargetRequirement TargetRequirement => TargetRequirement.None;
        
        public override void OnProceed(Order order) { }

        public override void OnUpdate(Order order) { }
        
        public override void Dispose(Order order) { }
        
        public override bool IsCarriedOut(Order order) => true;
    }
}