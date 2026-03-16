using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Abilities;
using Gameplay.Units;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    [CreateAssetMenu(menuName = "Gameplay Data/Orders/Ability Order", order = 0)]
    public class AbilityOrder : OrderType
    {
        [Space]
        [Expandable] [SerializeField] private AbilityType _abilityType;
        [Expandable] [SerializeField] private float _aoeEllipseRadius;

        public AbilityType AbilityType => _abilityType;
        public float AoeEllipseRadius => _aoeEllipseRadius;

        public override TargetRequirement TargetRequirement => _abilityType.TargetRequirement;

        public override string DisplayType => "Order — Ability";

        public override string DisplayDescription
        {
            get
            {
                string result = "";
                result += "<stat>";
                if (AbilityType.EnergyCost > 0)
                {
                    result += $"{AbilityType.EnergyCost} energy\n";
                }
                if (AbilityType.Cooldown > 0)
                {
                    string cooldonwnSec = Mathf.Round(Time.fixedDeltaTime * AbilityType.Cooldown).ToString("F1");
                    result += $"Cooldown: {cooldonwnSec} sec.\n";
                }
                if (AbilityType.MaxDistance > 0)
                    result += $"Distance: {AbilityType.MaxDistance}m\n";
                result += "</stat>";
                result += RawDisplayDescription;
                return result;
            }
        }

        public override bool CanBeIssued(Order order)
        {
            Ability ability = GetAbilityForOrder(order);
            return AbilityType.CanBeCast(ability, order.Target);
        }

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            Ability ability = GetAbilityForOrder(order);
            if ( ! ability.CanBeCast(order.Target))
                return;

            while (true)
            {
                await UniTask.WaitForFixedUpdate();
                
                Vector2 destination = order.Target.Unit ? order.Target.Unit.Position : order.Target.Point;
        
                if ( ! AbilityType.IsTargetInRadius(order.Actor, order.Target))
                {
                    order.Actor.Movement.Move(destination);
                    continue;
                }
                order.Actor.Movement.Stop();
                
                float angleToTarget = order.Actor.Position.DirectionTo(destination).ToDegrees();
                if (ability.Type.MustLookAtTarget && ! order.Actor.Direction.LookAngle.Equals(angleToTarget))
                {
                    order.Actor.Direction.RotateTowards(angleToTarget);
                    continue;
                }
        
                if (await order.Actor.Abilities.TryCast(ability, order.Target))
                    return;
            }
        }

        protected override void Dispose(Order order)
        {
            order.Actor.Movement.Stop();
        }

        private Ability GetAbilityForOrder(Order order)
        {
            return order.Actor.Abilities.GetAbility(AbilityType);
        }
    }
}