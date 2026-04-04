using System;
using Extentions;
using Extentions.Pause;
using UniRx;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitLife : UnitComponent
    {
        private readonly Timer _shieldRestorationTimer;
        
        private float _hitPoints;
        private float _shieldPoints;

        public int HitPoints => Mathf.FloorToInt(_hitPoints);
        public int MissingHitPoints => MaxHitPoints - HitPoints;
        public int ShieldPoints => Mathf.FloorToInt(_shieldPoints);
        public int MissingShieldPoints => MaxShieldPoints - ShieldPoints;

        public int MaxHitPoints => UnitType.MaxHitPoints; 
        public int MaxShieldPoints => UnitType.MaxShieldPoints;
        
        public float HitPercent => (float) HitPoints / MaxHitPoints;
        public bool HasShieldPoints => MaxShieldPoints > 0;
        public float ShieldPercent => HasShieldPoints ? (float) ShieldPoints / MaxShieldPoints : 0;

        public bool IsAlive => HitPoints >= 1;
        public bool IsDead => ! IsAlive;
        
        public Unit LastDamageDealer { get; private set; }
        public float LastDamageTime { get; private set; } = -1000;
        
        public event Action HitPointsOver;
        public event Action<int> DamageTaken;
        public event Action<int> HitPointsLost;
        public event Action<int> ShieldPointsLost;

        public UnitLife(Unit unit, IPauseReadonly tacticalPause) : base(unit)
        {
            _hitPoints = MaxHitPoints;
            _shieldPoints = MaxShieldPoints;
            
            if (UnitType.ShieldRestoreDelay > 0)
                _shieldRestorationTimer = new Timer(UnitType.ShieldRestoreDelay, tacticalPause);
            
            Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => RestoreShieldPoints());
        }

        public void TakeDamage(int damage, Unit damageDealer)
        {
            if (IsDead || damage <= 0)
                return;
            
            int shieldDamage = Mathf.Min(damage, ShieldPoints);
            int hitDamage = Mathf.Min(damage - shieldDamage, HitPoints);
            
            _hitPoints -=  hitDamage;
            _shieldPoints -=  shieldDamage;

            LastDamageDealer = damageDealer;
            LastDamageTime = Time.fixedTime;
            DamageTaken?.Invoke(damage);
            HitPointsLost?.Invoke(hitDamage);
            ShieldPointsLost?.Invoke(shieldDamage);
            
            if (HitPoints < 1)
                HitPointsOver?.Invoke();
            
            _shieldRestorationTimer?.Restart();
        }

        public void RestoreHitPoints(int value)
        {
            if (value <= 0)
                return;
            _hitPoints = Mathf.Min(_hitPoints + value, MaxHitPoints);
        }

        public void RestoreShieldPoints(int value)
        {
            if (value <= 0)
                return;
            _shieldPoints = Mathf.Min(_shieldPoints + value, MaxShieldPoints);
        }

        private void RestoreShieldPoints()
        {
            if (_shieldRestorationTimer?.IsOn ?? false)
                return;
            _shieldPoints += Time.fixedDeltaTime * UnitType.ShieldPointsPerSecond;
            _shieldPoints = Mathf.Min(_shieldPoints, MaxShieldPoints);
        }
    }
}