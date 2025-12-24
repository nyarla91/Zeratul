using Extentions;
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

        public override string DisplayType => "Order — Ability";

        public override string DisplayDescription
        {
            get
            {
                string result = "";
                result += "<color=grey>";
                if (AbilityType.Cooldown > 0)
                    result += $"Cooldown: {Time.fixedDeltaTime * AbilityType.Cooldown} sec.\n";
                if (AbilityType.MaxDistance > 0)
                    result += $"Distance: {AbilityType.MaxDistance}m\n";
                result += "</color>";
                result += RawDisplayDescription;
                return result;
            }
        }

        public override void OnProceed(Order order)
        {
            Ability ability = GetAbilityForOrder(order);
            if ( ! ability.IsReady)
                Complete(order);
        }

        public override void OnUpdate(Order order)
        {
            Ability ability = GetAbilityForOrder(order);
            Vector2 destination = order.Target.Unit ? order.Target.Unit.transform.position : order.Target.Point;
            
            if (!IsTargetInRadius(order))
            {
                order.Actor.Movement.Move(destination);
                return;
            }
            order.Actor.Movement.Stop();
            
            float angleToTarget = order.Actor.transform.DirectionTo2D(destination).ToDegrees();
            if (Mathf.DeltaAngle(order.Actor.Movement.LookAngle, angleToTarget) > AbilityType.MaxAngleToTarget)
            {
                order.Actor.Movement.RotateTowards(angleToTarget);
                return;
            }
            
            if (AbilityType.TryCast(ability, order.Target))
                Complete(order);
        }

        public override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }

        public override bool CanBeIssued(Order order)
        {
            Ability ability = GetAbilityForOrder(order);
            return AbilityType.CanBeCast(ability, order.Target);
        }

        private Ability GetAbilityForOrder(Order order)
        {
            return order.Actor.Abilities.GetAbility(AbilityType);
        }

        private bool IsTargetInRadius(Order order)
        {
            return TargetRequirement switch
            {
                TargetRequirement.None => true,
                TargetRequirement.Unit => Isometry.Distance(order.Actor.transform.position,
                    order.Target.Unit.transform.position) < AbilityType.MaxDistance,
                _ => Isometry.Distance(order.Actor.transform.position, order.Target.Point) < AbilityType.MaxDistance
            };
        }
    }
}