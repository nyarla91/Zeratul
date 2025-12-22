using System;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Units
{
    public class UnitLife : UnitComponent
    {
        public int HitPoints { get; private set; }
        public int ShieldPoints { get; private set; }

        public int MaxHitPoints => UnitType.Life.MaxHitPoints; 
        public int MaxShieldPoints => UnitType.Life.MaxShieldPoints;
        
        public float HitPercent => (float) HitPoints / MaxHitPoints;
        public bool HasShieldPoints => MaxShieldPoints > 0;
        public float ShieldPercent => HasShieldPoints ? 1 : (float) ShieldPoints / MaxShieldPoints;

        private Timer _shieldRecoveryTimer;

        public event Action OnHitPointsOver;
        
        [Inject] private IPauseRead PauseRead { get; set; }

        public void Init(UnitType unitType)
        {
            HitPoints = MaxHitPoints;
            ShieldPoints = MaxShieldPoints;
            _shieldRecoveryTimer = new Timer(this, unitType.Life.ShieldsRecoveryDelay, PauseRead);
            _shieldRecoveryTimer.Expired += RecoverShields;
        }

        private void RecoverShields()
        {
            ShieldPoints = MaxShieldPoints;
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
            
            _shieldRecoveryTimer.Restart();
        }

        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TakeDamage(5);
            }
        }
    }
}