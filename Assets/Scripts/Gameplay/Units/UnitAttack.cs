using System;
using System.Collections.Generic;
using Extentions;
using Extentions.Pause;
using Gameplay.Data.Configs;
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

        private UnitWeaponType Weapon => UnitType.WeaponType;

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

            Unit.Orders.CurrentOrderUpdated += _ => CurrentTarget = null;

            if (UnitType.WeaponType.AutoAttack)
            {
                IDisposable autoAttackSubscription = Observable.EveryFixedUpdate()
                    .Where(_ => tacticalPause.IsUnpaused)
                    .Subscribe(_ => UpdateAutoAttackTarget());
                Unit.Killed += autoAttackSubscription.Dispose;
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
            return Isometry.Distance(Unit.Position, other.Position) < Weapon.MaxDistance;
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
                    Unit.Movement.Move(CurrentTarget.Position);
                else
                    StopAttacking();
                return;
            }
            Unit.Movement?.Stop();
                
            float targetAngle = (Unit.Position.DirectionTo(CurrentTarget.Position) / Isometry.Scale).ToDegrees();
            Unit.Direction.RotateTowards(targetAngle);
            if ( ! Mathf.Approximately(Unit.Direction.LookAngle, targetAngle))
                return;
                
            StrikeUnit(CurrentTarget);
        }

        private void UpdateAutoAttackTarget()
        {
            if (Unit.Ownership.OwnedByEnemy || ! Unit.Orders.IsIdle || (Unit.Movement?.IsHoldingPosition ?? false))
                return;

            Debug.Log(Unit.AI.PreferredAttackTarget);
            CurrentTarget = Unit.AI.PreferredAttackTarget;
        }

        private async void StrikeUnit(Unit target)
        {
            if ( ! await Unit.Stagger.TryBegin(Weapon.WindupTime, Weapon.RecoveryTime, "attack"))
                return;
            target.Life.TakeDamage(Weapon.BaseDamage, Unit);
        }
    }
}