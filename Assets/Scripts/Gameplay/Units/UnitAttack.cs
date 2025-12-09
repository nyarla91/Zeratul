using System.Collections;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitAttack : UnitComponent
    {
        private UnitWeapon[] _weapons;
        private Coroutine _attackCoroutine;
        
        public bool IsAttacking => _attackCoroutine != null;
        
        [Inject] private IPauseRead PauseRead { get; set; }

        private void Awake()
        {
            _weapons = new UnitWeapon[UnitType.Weapons.Length];
            for (int i = 0; i < _weapons.Length; i++)
            {
                Timer cooldown = new(this, UnitType.Weapons[i].Cooldown, PauseRead);
                _weapons[i] = new UnitWeapon(UnitType.Weapons[i], cooldown);
            }
        }

        public void StartAttacking(Unit target)
        {
            _attackCoroutine = StartCoroutine(Attacking(target));
            Composition.Movement.Stop();
        }

        public void StopAttacking()
        {
            _attackCoroutine?.Stop(this);
            _attackCoroutine = null;
        }

        private IEnumerator Attacking(Unit target)
        {
            UnitWeapon currentWeapon = _weapons[0];
            while (true)
            {
                yield return new WaitForFixedUpdate();
                
                if (Vector3.Distance(transform.position, target.transform.position) > currentWeapon.Type.MaxDistance)
                {
                    Composition.Movement.Move(target.transform.position);
                    continue;
                }
                Composition.Movement.Stop();
                
                float targetAngle = (transform.position.DirectionTo(target.transform.position) / Isometry.Scale).ToDegrees();
                Composition.Movement.RotateTowards(targetAngle, Time.fixedDeltaTime);
                if (!Mathf.Approximately(Composition.Movement.LookAngle, targetAngle))
                    continue;
                
                if ( ! currentWeapon.Cooldown.IsIdle)
                    continue;
                
                target.Life.TakeDamage(currentWeapon.Type.BaseDamage);
                currentWeapon.Cooldown.Restart();
            }
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