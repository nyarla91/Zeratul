using System.Collections;
using Gameplay.Data;
using Source.Extentions;
using Source.Extentions.Pause;
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
                if (Vector3.Distance(transform.position, target.transform.position) > currentWeapon.Type.MaxDistance)
                {
                    Composition.Movement.Move(target.transform.position);
                    yield return new WaitForFixedUpdate();
                    continue;
                }
                Composition.Movement.Stop();
                if (currentWeapon.Cooldown.IsIdle)
                {
                    target.Life.TakeDamage(currentWeapon.Type.BaseDamage);
                    currentWeapon.Cooldown.Restart();
                }
                yield return new WaitForFixedUpdate();
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