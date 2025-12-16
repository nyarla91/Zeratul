using System;
using System.Collections;
using System.Linq;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Orders;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitAttack : UnitComponent
    {
        [SerializeField] private OrderType _attackOrder;
        
        private UnitWeapon _weapon;
        private Coroutine _attackCoroutine;

        private bool CanAttack => UnitType.AvailableOrders.Contains(_attackOrder);
        
        public bool IsAttacking => _attackCoroutine != null;
        
        [Inject] private IsometricOverlap IsometricOverlap { get; set; }
        [Inject] private IPauseRead PauseRead { get; set; }

        public void Init(UnitType unitType)
        {
            Timer cooldown = new(this, unitType.Weapon.Cooldown, PauseRead);
            _weapon = new UnitWeapon(unitType.Weapon, cooldown);
        }

        public void StartAttacking(Unit target)
        {
            if ( ! CanAttack)
                return;
            StopAttacking();
            _attackCoroutine = StartCoroutine(Attacking(target));
        }

        public void StopAttacking()
        {
            if ( ! IsAttacking)
                return;
            _attackCoroutine?.Stop(this);
            _attackCoroutine = null;
        }

        private IEnumerator Attacking(Unit target)
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                
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
            
        }

        private void FixedUpdate()
        {
            TryAutoAttack();
        }

        private void TryAutoAttack()
        {
            if ( ! CanAttack || ! UnitType.Weapon.AutoAttack || ! Composition.Orders.IsIdle || IsAttacking)
                return;
            if ( ! IsometricOverlap.TryGetUnits(transform.position, UnitType.SightRadius, out Unit[] units))
                return;
            
            units = units.Where(unit => unit.Ownership.OwnedByPlayer != Composition.Ownership.OwnedByPlayer).ToArray();
            if (units.Length == 0)
                return;
            
            Unit closestTarget = units.MinElement(unit => Isometry.Distance(transform.position, unit.transform.position));
            OrderTarget target = new(default, closestTarget);
            Composition.Orders.IssueOrder(new Order(_attackOrder, Composition, target), false);
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