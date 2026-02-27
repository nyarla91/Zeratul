using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitAttack : UnitComponent
    {
        [SerializeField] private OrderType _attackOrder;
        [SerializeField] private UnitValidatorGroup _autoAttackValidators;

        public bool IsAbleToAttack => UnitType.WeaponType && UnitType.AvailableOrders.Contains(_attackOrder) || Unit.Stagger.IsStaggered;
        
        public Unit CurrentTarget { get; private set; }
        public bool IsAttacking => CurrentTarget != null;

        private UnitWeaponType Weapon => UnitType.WeaponType;

        public Unit ClosestTarget
        {
            get
            {
                HashSet<Unit> units = Unit.Sight.VisibleUnits(_autoAttackValidators);
                return units?.MinElement(unit => Isometry.Distance(Unit.Position, unit.Position));
            }
        }
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        public void Init(UnitType unitType)
        {
            if ( ! IsAbleToAttack)
                return;
        }

        public void StartAttacking(Unit target)
        {
            if ( ! IsAbleToAttack || ! CanAttackUnit(target))
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
            => target && target != Unit && target.Visibility.CanBeTargetedBy(Unit) && target.Life.IsAlive;

        private void TryAutoAttack()
        {
            if ( ! IsAbleToAttack || ! UnitType.WeaponType.AutoAttack || IsAttacking || ! Unit.Orders.IsIdle) return;

            Unit closestTarget = ClosestTarget;
            if ( ! closestTarget) return;
            OrderTarget target = new(default, closestTarget);
            Unit.Orders.IssueOrder(new Order(_attackOrder, Unit, target), false);
        }

        private void FixedUpdate()
        {
            if (TacticalPause.IsPaused || ! IsAbleToAttack)
                return;
            
            TryAutoAttack();
            
            if ( ! IsAttacking)
                return;

            if ( ! CanAttackUnit(CurrentTarget))
            {
                StopAttacking();
                return;
            }
                    
            if (Vector3.Distance(Unit.Position, CurrentTarget.Position) > Weapon.MaxDistance)
            {
                Unit.Movement.Move(CurrentTarget.Position);
                return;
            }
            Unit.Movement.Stop();
                
            float targetAngle = (Unit.Position.DirectionTo(CurrentTarget.Position) / Isometry.Scale).ToDegrees();
            Unit.Movement.RotateTowards(targetAngle);
            if ( ! Mathf.Approximately(Unit.Movement.LookAngle, targetAngle))
                return;
                
            AttackUnit(CurrentTarget);
        }

        private async void AttackUnit(Unit target)
        {
            if ( ! await Unit.Stagger.TryBegin(Weapon.WinduoTime, Weapon.RecoveryTime, "attack"))
                return;
            target.Life.TakeDamage(Weapon.BaseDamage);
        }
    }
}