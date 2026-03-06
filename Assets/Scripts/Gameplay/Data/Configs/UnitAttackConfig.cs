using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Unit Attack Config", order = 0)]
    public class UnitAttackConfig : ScriptableObject
    {
        [SerializeField] private OrderType _defaultAttackOrder;
        [SerializeField] private UnitValidatorGroup _autoAttackValidators;

        public OrderType DefaultAttackOrder => _defaultAttackOrder;
        public UnitValidatorGroup AutoAttackValidators => _autoAttackValidators;
    }
}