using System.Collections.Generic;
using System.Linq;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Configs;
using UniRx;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitAttack : UnitComponent
    {
        private readonly UnitAttackConfig _config;
        
        public Unit CurrentTarget { get; private set; }
        public bool IsAttacking => CurrentTarget != null;

        private UnitWeaponType Weapon => UnitType.WeaponType;

        public Unit ClosestTarget
        {
            get
            {
                HashSet<Unit> units = Unit.Sight.VisibleUnits(_config.AutoAttackValidators);
                return units?.MinElement(unit => Isometry.Distance(Unit.Position, unit.Position));
            }
        }

        public UnitAttack(Unit unit, IPauseReadonly tacticalPause, UnitAttackConfig config) : base(unit)
        {
            _config = config;

            if ( ! UnitType.WeaponType)
                return;
            
            Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused)
                .Where(_ => IsAttacking)
                .Subscribe(_ => UpdateAttack());
            
            if (UnitType.WeaponType.AutoAttack)
                Observable.EveryFixedUpdate()
                    .Where(_ => tacticalPause.IsUnpaused)
                    .Where(_ => ! IsAttacking)
                    .Subscribe(_ => TryAutoAttack());
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

        public bool CanAttackUnit(Unit target)
        {
            if (target == Unit)
                return false;
            if (target.Life.IsDead)
                return false;
            if (UnitType.IsImmobile && ! IsUnitInRange(target))
                return false;
            if ( ! target.Visibility.CanBeTargetedBy(Unit))
                return false;
            return true;
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

        private void TryAutoAttack()
        {
            if (IsAttacking || ! Unit.Orders.IsIdle)
                return;
            
            Unit closestTarget = ClosestTarget;
            if ( ! closestTarget) return;
            OrderTarget target = new(default, closestTarget);
            Unit.Orders.IssueOrder(new Order(_config.DefaultAttackOrder, Unit, target), false);
        }

        private bool IsUnitInRange(Unit other)
        {
            return Isometry.Distance(Unit.Position, other.Position) < Weapon.MaxDistance;
        }

        private async void StrikeUnit(Unit target)
        {
            if ( ! await Unit.Stagger.TryBegin(Weapon.WindupTime, Weapon.RecoveryTime, "attack"))
                return;
            target.Life.TakeDamage(Weapon.BaseDamage);
        }
    }
}