using System.Linq;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Order", order = 0)]
    public class UnitOrderValidator : UnitValidator
    {
        [SerializeField] private OrderType _orderType;
        
        public override bool IsValid(Unit actor, Unit target)
        {
            return target.Type.AvailableOrders.Contains(_orderType);
        }
    }
}