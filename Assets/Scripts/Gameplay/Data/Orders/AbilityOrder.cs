using Gameplay.Data.Abilities;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Ability Order", order = 0)]
    public class AbilityOrder : OrderType
    {
        [Space]
        [SerializeField] private AbilityType _abilityType;

        public AbilityType AbilityType => _abilityType;

        public override TargetRequirement TargetRequirement => _abilityType.TargetRequirement;
        
        public override void OnProceed(Order order)
        {
            
        }

        public override void OnUpdate(Order order)
        {
            Debug.Log($"{order.Actor.Type} using {AbilityType} ability targeting {order.Target.Point} {order.Target.Unit}");
        }

        public override void Dispose(Order order)
        {
            
        }
    }
}