using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Issue Order to Caster", order = 0)]
    public class IssueOrderToCasterEffect : EffectTargetingPoint
    {
        [SerializeField] private OrderType _orderType;
        [SerializeField] private bool _onlyIfQueueIsClear;
        
        public override void Apply(Unit caster, Unit target) => IssueOrder(caster, OrderTarget.FromUnit(target));

        public override void Apply(Unit caster, Vector2 target) => IssueOrder(caster, OrderTarget.FromPoint(target));

        private void IssueOrder(Unit caster, OrderTarget orderTarget)
        {
            if (_onlyIfQueueIsClear && caster.Orders.OrdersQueue.Length > 0)
                return;
            Order order = new(_orderType, caster, orderTarget);
            caster.Orders.IssueOrder(order, true);
        }
    }
}