using System;
using _Core.Pause;
using Save.Data.Units;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitLife : UnitComponent
    {
        protected override string LoadKey => UnitLifeSaveSystem.LoadKey;

        private readonly GameTime _gameTime;
        private readonly IGetUnitByIdService _getUnitByIdService;
        private float _hitPoints;
        private float _shieldPoints;

        public int HitPoints => Mathf.FloorToInt(_hitPoints);
        public int MaxHitPoints => UnitType.MaxHitPoints; 
        public float HitPercent => (float) HitPoints / MaxHitPoints;
        public int MissingHitPoints => MaxHitPoints - HitPoints;
        
        public int ShieldPoints => Mathf.FloorToInt(_shieldPoints);
        public int MaxShieldPoints => UnitType.MaxShieldPoints;
        public float ShieldPercent => HasShieldPoints ? (float) ShieldPoints / MaxShieldPoints : 0;
        public int MissingShieldPoints => MaxShieldPoints - ShieldPoints;
        public bool HasShieldPoints => MaxShieldPoints > 0;
        public bool AreShieldsRestoring => HasShieldPoints && _gameTime.Frame - LastDamageFrame >= UnitType.ShieldRestoreDelay;

        public int LifePoints => HitPoints + ShieldPoints;
        public int MaxLifePoints => MaxHitPoints + ShieldPoints;
        public float LifePercent => (float) LifePoints / MaxLifePoints;
        public int MissingLifePoints => MaxLifePoints - LifePoints;
        
        public Unit LastDamageDealer { get; private set; }
        public int LastDamageFrame { get; private set; } = -1000;
        
        public event Action<int> DamageTaken;
        public event Action<int> HitPointsLost;
        public event Action<int> ShieldPointsLost;

        public UnitLife(Unit unit, GameTime gameTime, IPauseReadonly tacticalPause, IGetUnitByIdService getUnitByIdService) : base(unit)
        {
            _gameTime = gameTime;
            _getUnitByIdService = getUnitByIdService;
            _hitPoints = MaxHitPoints;
            _shieldPoints = MaxShieldPoints;
            
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => RestoreShieldPoints());
        }

        public override IUnitSaveSystem Save()
        {
            int lastDamageDealerId = LastDamageDealer?.Id ?? -1;
            return new UnitLifeSaveSystem(_hitPoints, _shieldPoints, lastDamageDealerId, LastDamageFrame);
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitLifeSaveSystem system = GetSaveSystem<UnitLifeSaveSystem>(saveData);
            _hitPoints = system.hitPoints;
            _shieldPoints = system.shieldPoints;
            LastDamageDealer = _getUnitByIdService.GetUnitById(system.lastDamageDealerId);
            LastDamageFrame = system.lastDamageFrame;
        }

        public void TakeDamage(int damage, Unit damageDealer)
        {
            if (Unit.IsDead || damage <= 0)
                return;
            
            int shieldDamage = Mathf.Min(damage, ShieldPoints);
            int hitDamage = Mathf.Min(damage - shieldDamage, HitPoints);
            
            _hitPoints -=  hitDamage;
            _shieldPoints -=  shieldDamage;

            LastDamageDealer = damageDealer;
            LastDamageFrame = _gameTime.Frame;
            DamageTaken?.Invoke(damage);
            HitPointsLost?.Invoke(hitDamage);
            ShieldPointsLost?.Invoke(shieldDamage);
            
            if (HitPoints < 1)
                Unit.Kill();
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
            if ( ! AreShieldsRestoring)
                return;
            _shieldPoints += Time.fixedDeltaTime * UnitType.ShieldPointsPerSecond;
            _shieldPoints = Mathf.Min(_shieldPoints, MaxShieldPoints);
        }
    }
}