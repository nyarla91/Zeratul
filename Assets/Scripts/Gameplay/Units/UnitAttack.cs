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
        
        private UnitWeapon _weapon;

        public bool IsAbleToAttack => UnitType.WeaponType && UnitType.AvailableOrders.Contains(_attackOrder);
        
        public Unit CurrentTarget { get; private set; }
        public bool IsAttacking => CurrentTarget != null;

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
            Timer cooldown = new(this, unitType.WeaponType.Cooldown, TacticalPause);
            _weapon = new UnitWeapon(unitType.WeaponType, cooldown);
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
            if (TacticalPause.IsPaused)
                return;
            
            TryAutoAttack();
            
            if ( ! IsAttacking)
                return;

            if ( ! CanAttackUnit(CurrentTarget))
            {
                StopAttacking();
                return;
            }
                    
            if (Vector3.Distance(Unit.Position, CurrentTarget.Position) > _weapon.Type.MaxDistance)
            {
                Unit.Movement.Move(CurrentTarget.Position);
                return;
            }
            Unit.Movement.Stop();
                
            float targetAngle = (Unit.Position.DirectionTo(CurrentTarget.Position) / Isometry.Scale).ToDegrees();
            Unit.Movement.RotateTowards(targetAngle);
            if ( ! Mathf.Approximately(Unit.Movement.LookAngle, targetAngle))
                return;
                
            if ( ! _weapon.Cooldown.IsIdle)
                return;
                
            CurrentTarget.Life.TakeDamage(_weapon.Type.BaseDamage);
            _weapon.Cooldown.Restart();
        }
    }

    public class UnitWeapon
    {
        public UnitWeaponType Type { get; }
        public Timer Cooldown { get; }

        public UnitWeapon(UnitWeaponType type, Timer cooldown)
        {
            Type = type;
            Cooldown = cooldown;
        }
    }
}