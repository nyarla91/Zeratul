using System.Threading;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Abilities;
using Gameplay.Data.Configs;
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
        [SerializeField] private ReferenceIRadiusSource _aoeEllipseRadius;
        [SerializeField] private OrderErrorConfig _errors;

        public AbilityType AbilityType => _abilityType;

        public float AoeEllipseRadius => _aoeEllipseRadius.I?.Radius ?? 0;

        public override TargetRequirement TargetRequirement => _abilityType.TargetRequirement;

        public override string DisplayType => Localizer.Translate("order-ability");

        public override string DisplayDescription
        {
            get
            {
                string result = "";
                result += "<stat>";
                if (AbilityType.EnergyCost > 0)
                {
                    result += Localizer.Translate("ability-stat-energy").Replace("#", AbilityType.EnergyCost.ToString()) + "\n";
                }
                if (AbilityType.Cooldown > 0)
                {
                    string cooldonwnSec = Mathf.Round(Time.fixedDeltaTime * AbilityType.Cooldown).ToString("F1");
                    result += Localizer.Translate("ability-stat-cooldown").Replace("#", AbilityType.Cooldown.FramesToSeconds()) + "\n";
                }

                if (AbilityType.MaxDistance > 0)
                {
                    result += Localizer.Translate("ability-stat-range").Replace("#", AbilityType.MaxDistance.ToString()) + "\n";
                }
                result += "</stat>";
                result += RawDisplayDescription;
                return result;
            }
        }

        public override bool IsActorValid(Unit actor, out string errorMessage)
        {
            if ( ! AbilityType.IgnoreLock && actor.Abilities.IsLocked)
            {
                errorMessage = _errors.Locked;
                return false;
            }

            Ability ability = actor.Abilities.GetAbility(AbilityType);
            if ( ! ability.IsReady)
            {
                int framesLeft = ability.CooldownLeft;
                errorMessage = _errors.NotReadyAbility.Replace("#", framesLeft.FramesToSeconds());
                return false;
            }
            if (actor.Abilities.EnergyPoints < AbilityType.EnergyCost)
            {
                int missingEnergy = AbilityType.EnergyCost - actor.Abilities.EnergyPoints;
                errorMessage = _errors.NotEnoughEnergy.Replace("#", missingEnergy.ToString());
                return false;
            }
            if (AbilityType.CasterValidators.IsInvalid(actor, actor, out errorMessage))
            {
                return false;
            }
            errorMessage = null;
            return true;
        }

        public override bool IsTargetValid(Unit actor, OrderTarget target, out string errorMessage)
        {
            if (target.Unit && ! target.Unit.Visibility.CanBeTargetedBy(actor))
            {
                errorMessage = Localizer.Translate(_errors.TargetInvalid);
                return false;
            }
            if (target.Unit && AbilityType.TargetValidators.IsInvalid(actor, target.Unit, out errorMessage))
            {
                errorMessage = Localizer.Translate(errorMessage);
                return false;
            }
            if ( ! actor.CanMove && ! AbilityType.IsTargetInRadius(actor, target))
            {
                errorMessage = _errors.OutOfRange;
                return false;
            }
            errorMessage = null;
            return true;
        }

        protected override async UniTask CarryOutBody(Order order, CancellationToken ct)
        {
            Ability ability = GetAbilityForOrder(order);
            if ( ! ability.CanBeCast(order.Target))
                return;

            while (true)
            {
                await UniTask.WaitForFixedUpdate(ct);
                
                Vector2 destination = order.Target.Unit ? order.Target.Unit.Position : order.Target.Point;
        
                if ( ! AbilityType.IsTargetInRadius(order.Actor, order.Target))
                {
                    order.Actor.Movement?.Move(destination, AbilityType.MaxDistance);
                    continue;
                }
                order.Actor.Movement?.Stop();
                
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