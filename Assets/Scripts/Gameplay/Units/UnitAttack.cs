using System;
using System.Collections;
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
                Unit[] units = Composition.Sight.VisibleUnits(_autoAttackValidators);
                return units?.MinElement(unit => Isometry.Distance(transform.position, unit.transform.position));
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
            => target && target != Composition && target.Visibility.CanBeTargetedBy(Composition) && target.Life.IsAlive;

        private void TryAutoAttack()
        {
            if ( ! IsAbleToAttack || ! UnitType.WeaponType.AutoAttack || IsAttacking || ! Composition.Orders.IsIdle) return;

            Unit closestTarget = ClosestTarget;
            if ( ! closestTarget) return;
            OrderTarget target = new(default, closestTarget);
            Composition.Orders.IssueOrder(new Order(_attackOrder, Composition, target), false);
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
                    
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) > _weapon.Type.MaxDistance)
            {
                Composition.Movement.Move(CurrentTarget.transform.position);
                return;
            }
            Composition.Movement.Stop();
                
            float targetAngle = (transform.position.DirectionTo(CurrentTarget.transform.position) / Isometry.Scale).ToDegrees();
            Composition.Movement.RotateTowards(targetAngle);
            if ( ! Mathf.Approximately(Composition.Movement.LookAngle, targetAngle))
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