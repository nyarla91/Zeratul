using System;
using Extentions;
using Extentions.Pause;
using Gameplay.Data.Configs;
using Gameplay.Data.Effects;
using Gameplay.Data.Units;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitAttack : UnitComponent
    {
        private readonly UnitAttackConfig _config;
        private readonly OrderErrorConfig _errors;

        public Unit CurrentTarget { get; private set; }
        public bool IsAttacking => CurrentTarget != null;

        public Modifier AttackSpeedModifier { get; } = new();

        private UnitWeaponType Weapon => UnitType.WeaponType;
        
        public event Action<Unit> Struck;

        public UnitAttack(Unit unit, IPauseReadonly tacticalPause, UnitAttackConfig config, OrderErrorConfig errors) : base(unit)
        {
            _config = config;
            _errors = errors;

            if ( ! UnitType.WeaponType)
                return;
            
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Where(_ => IsAttacking)
                .Subscribe(_ => UpdateAttack());

            Unit.Orders.LeftIdle += StopAttacking;

            if (UnitType.WeaponType.AutoAttackDistance > 0)
            {
                Unit.FixedUpdateAsObservable()
                    .Where(_ => tacticalPause.IsUnpaused)
                    .Subscribe(_ => UpdateAutoAttack());
            }
        }

        public void StartAttacking(Unit target)
        {
            if ( ! CanAttackUnit(target))
                return;
            StopAttacking();
            CurrentTarget = target;
        }

        public void StopAttacking()
        {
            if ( ! IsAttacking)
                return;
            CurrentTarget = null;
            Unit.Movement?.Stop();
        }

        public bool CanAttackUnit(Unit target) => CanAttackUnit(target, out _);
        
        public bool CanAttackUnit(Unit target, out string errorMessage)
        {
            errorMessage = _errors.TargetInvalid;
            if (target == Unit)
            {
                errorMessage = _errors.CantTargetSelf;
                return false;
            }
            
            if (target.IsDead)
                return false;
            
            if (UnitType.IsImmobile && !IsUnitInRange(target))
            {
                errorMessage = _errors.OutOfRange;
                return false;
            }
            
            if ( ! target.Visibility.CanBeTargetedBy(Unit))
                return false;
            
            return true;
        }

        public bool IsUnitInRange(Unit other)
        {
            return Isometry.Distance(Unit.Position, other) < Weapon.MaxDistance;
        }

        private void UpdateAttack()
        {
            if ( ! CanAttackUnit(CurrentTarget))
            {
                StopAttacking();
                return;
            }

            if ( ! IsUnitInRange(CurrentTarget))
            {
                if (Unit.CanMove)
                    Unit.Movement.Move(CurrentTarget.Position, Weapon.MaxDistance);
                else
                    StopAttacking();
                return;
            }
            Unit.Movement?.Stop();
                
            float targetAngle = (Unit.Position.DirectionTo(CurrentTarget.Position) / Isometry.Scale).ToDegrees();
            Unit.Direction.RotateTowards(targetAngle);
            if (Mathf.Abs(Unit.Direction.LookAngle - targetAngle) > _config.DeltaAngleTolerance)
                return;
            if (Unit.Abilities.IsLocked)
                return;
                
            StrikeUnit(CurrentTarget);
        }

        private void UpdateAutoAttack()
        {
            if (Unit.Alliance.OwnedByEnemy || ! Unit.Orders.IsIdle || (Unit.Movement?.IsHoldingPosition ?? false))
                return;

            Unit target = Unit.AI.PreferredAttackTarget;
            if (target != null && Isometry.Distance(Unit.Position, target) < Weapon.AutoAttackDistance)
                StartAttacking(target);
            else
                StopAttacking();
        }

        private async void StrikeUnit(Unit target)
        {
            float staggerMultiplier = 1 / AttackSpeedModifier.Value;
            int windupTime = Mathf.RoundToInt(Weapon.WindupTime * staggerMultiplier);
            int recoveryTime = Mathf.RoundToInt(Weapon.RecoveryTime * staggerMultiplier);
            if ( ! await Unit.Stagger.TryBegin(windupTime, recoveryTime, "attack"))
                return;
            target.Life.TakeDamage(Weapon.BaseDamage, Unit);
            foreach (EffectTargetingUnit effect in Weapon.AdditionalEffects)
            {
                effect.Apply(Unit, target);
            }
            Struck?.Invoke(target);
        }
    }
}