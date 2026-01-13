using System;
using Extentions;
using Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitLife : UnitComponent
    {
        private Timer _shieldRestorationTimer;
        private float _shieldRestorationRemain;
        
        public int HitPoints { get; private set; }
        public int ShieldPoints { get; private set; }

        public int MaxHitPoints => UnitType.MaxHitPoints; 
        public int MaxShieldPoints => UnitType.MaxShieldPoints;
        
        public float HitPercent => (float) HitPoints / MaxHitPoints;
        public bool HasShieldPoints => MaxShieldPoints > 0;
        public float ShieldPercent => HasShieldPoints ? (float) ShieldPoints / MaxShieldPoints : 1;

        public event Action OnHitPointsOver;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        public void Init(UnitType unitType)
        {
            HitPoints = MaxHitPoints;
            ShieldPoints = MaxShieldPoints;
            if (unitType.ShieldRestoreDelay > 0)
                _shieldRestorationTimer = new Timer(this, unitType.ShieldRestoreDelay, TacticalPause);
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                return;
            int shieldDamage = Mathf.Min(damage, ShieldPoints);
            int hitDamage = Mathf.Min(damage - shieldDamage, HitPoints);
            
            HitPoints -=  hitDamage;
            ShieldPoints -=  shieldDamage;
            
            if (HitPoints <= 0)
                OnHitPointsOver?.Invoke();
            
            _shieldRestorationTimer?.Restart();
        }

        private void FixedUpdate()
        {
            if (_shieldRestorationTimer == null || _shieldRestorationTimer.IsOn || TacticalPause.IsPaused)
                return;
            _shieldRestorationRemain += Time.fixedDeltaTime * UnitType.ShieldPointsPerSecond;
            _shieldRestorationRemain = Mathf.Min(_shieldRestorationRemain, MaxShieldPoints - ShieldPoints);
            ShieldPoints += (int) _shieldRestorationRemain;
            _shieldRestorationRemain %= 1;
        }
    }
}