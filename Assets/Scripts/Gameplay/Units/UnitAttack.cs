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
        private Coroutine _attackCoroutine;

        public bool IsAbleToAttack => UnitType.WeaponType && UnitType.AvailableOrders.Contains(_attackOrder);
        
        public bool IsAttacking => _attackCoroutine != null;

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
            if ( ! IsAbleToAttack)
                return;
            StopAttacking();
            _attackCoroutine = StartCoroutine(Attacking(target));
        }

        public bool TryAutoAttack()
        {
            if ( ! IsAbleToAttack || ! UnitType.WeaponType.AutoAttack || IsAttacking || ! Composition.Orders.IsIdle)
                return false;

            Unit closestTarget = ClosestTarget;
            if ( ! closestTarget)
                return false;
            OrderTarget target = new(default, closestTarget);
            Composition.Orders.IssueOrder(new Order(_attackOrder, Composition, target), false);
            return true;
        }

        public void StopAttacking()
        {
            if ( ! IsAttacking)
                return;
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        private IEnumerator Attacking(Unit target)
        { 
            while (true)
            {
                yield return new WaitForFixedUpdate();
                
                if ( ! target)
                    break;
                
                if ( ! target.Visibility.CanBeTargetedBy(Composition))
                    break;
                    
                if (Vector3.Distance(transform.position, target.transform.position) > _weapon.Type.MaxDistance)
                {
                    Composition.Movement.Move(target.transform.position);
                    continue;
                }
                Composition.Movement.Stop();
                
                float targetAngle = (transform.position.DirectionTo(target.transform.position) / Isometry.Scale).ToDegrees();
                Composition.Movement.RotateTowards(targetAngle);
                if (!Mathf.Approximately(Composition.Movement.LookAngle, targetAngle))
                    continue;
                
                if ( ! _weapon.Cooldown.IsIdle)
                    continue;
                
                target.Life.TakeDamage(_weapon.Type.BaseDamage);
                _weapon.Cooldown.Restart();
            }
            StopAttacking();
        }

        private void FixedUpdate()
        {
            TryAutoAttack();
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