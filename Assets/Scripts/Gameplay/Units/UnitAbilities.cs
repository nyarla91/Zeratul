using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Abilities;
using Gameplay.Data.Effects;
using Gameplay.Data.Orders;
using Save.Data.Units;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitAbilities : UnitComponent
    {
        protected override string LoadKey => UnitAbilitiesSaveSystem.LoadKey;

        private readonly GameTime _gameTime;
        private readonly GameDataRegistry _gameDataRegistry;
        private readonly HashSet<object> _lockSources = new();
        private readonly Dictionary<AbilityType, Ability> _abilities = new();

        private float _energyPoints;
        public int EnergyPoints => Mathf.FloorToInt(_energyPoints);

        public int MaxEnergyPoints => UnitType.MaxEnergyPoints; 
        
        public bool HasEnergyPoints => MaxEnergyPoints > 0;
        public float EnergyPercent => HasEnergyPoints ? (float) EnergyPoints / MaxEnergyPoints : 1;
        
        public int LastEnergySpentFrame { get; private set; }

        public bool IsLocked => _lockSources.Count > 0;
        public bool IsUnlocked => ! IsLocked;

        public event Action<AbilityType, OrderTarget> CastedAbility;

        private IPauseReadonly TacticalPause { get; set; }

        public UnitAbilities(Unit unit, GameTime gameTime, IPauseReadonly tacticalPause, GameDataRegistry gameDataRegistry) : base(unit)
        {
            _gameTime = gameTime;
            _gameDataRegistry = gameDataRegistry;
            TacticalPause = tacticalPause;
            
            Unit.FixedUpdateAsObservable()
                .Where(_ => TacticalPause.IsUnpaused)
                .Subscribe(_ => RestoreEnergyPoints());
            
            _energyPoints = MaxEnergyPoints;
            
            AbilityOrder[] abilityOrders = UnitType.AvailableOrders.OfType<AbilityOrder>().ToArray();
            
            if (abilityOrders.Length == 0)
                return;
            
            foreach (AbilityOrder abilityOrder in abilityOrders)
            {
                _abilities.Add(abilityOrder.AbilityType, new Ability(abilityOrder.AbilityType, Unit, gameTime));
            }
        }

        public override IUnitSaveSystem Save()
        {
            Dictionary<string, int> lastCastFrameByAbilityName = _abilities
                .ToDictionary(pair => pair.Key.name, pair => pair.Value.LastCastFrame);
            
            return new UnitAbilitiesSaveSystem(_energyPoints, LastEnergySpentFrame, lastCastFrameByAbilityName);
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitAbilitiesSaveSystem system = GetSaveSystem<UnitAbilitiesSaveSystem>(saveData);
            _energyPoints = system.energyPoints;
            LastEnergySpentFrame = system.lastEnergySpentFrame;

            foreach (KeyValuePair<string, int> pair in system.lastCastFrameByAbilityName)
            {
                AbilityType abilityType = _gameDataRegistry.Get<AbilityType>(pair.Key);
                _abilities[abilityType].LastCastFrame = pair.Value;
            }
        }
        
        public async UniTask<bool> TryCast(Ability ability, OrderTarget target)
        {
            if ( ! ability.CanBeCast(target))
                return false;
            
            AbilityType abilityType = ability.Type;
            
            if ( ! await Unit.Stagger.TryBegin(abilityType.WindupTime, abilityType.RecoveryTime, abilityType.AnimationAction))
                return false;

            if ( ! TrySpendEnergy(abilityType.EnergyCost))
                return false;

            foreach (EffectTargetingUnit effect in abilityType.CasterEffects)
            {
                effect.Apply(ability.Caster, ability.Caster);
            }

            if (target.Unit)
            {
                foreach (EffectTargetingUnit effect in abilityType.UnitTargetEffects)
                {
                    effect.Apply(ability.Caster, target.Unit);
                }
            }
            else
            {
                foreach (EffectTargetingPoint effect in abilityType.PointTargetEffects)
                {
                    effect.Apply(ability.Caster, target.Point);
                }
            }
            
            ability.StartCooldown();
            foreach (Ability sharedAbility in _abilities.Values)
            {
                if (sharedAbility.Type.CooldownGroup == null)
                    continue;
                if (sharedAbility.Type.CooldownGroup != ability.Type.CooldownGroup)
                    continue;
                sharedAbility.StartCooldown();
            }
            CastedAbility?.Invoke(abilityType, target);
            return true;
        }
        
        public void Lock(object source) => _lockSources.Add(source);
        
        public void Unlock(object source) => _lockSources.Remove(source);

        public bool TrySpendEnergy(int energy)
        {
            if (energy <= 0)
                return true;
            if (energy > EnergyPoints)
                return false;
            _energyPoints -= energy;
            LastEnergySpentFrame = _gameTime.Frame;
            return true;
        }

        public void RestoreEnergyPoints(int value)
        {
            if (value <= 0)
                return;
            _energyPoints = Mathf.Min(_energyPoints + value, MaxEnergyPoints);
        }
        
        public Ability GetAbility(AbilityType abilityType) => _abilities.GetValueOrDefault(abilityType);

        private void RestoreEnergyPoints()
        {
            if ( ! HasEnergyPoints || _gameTime.Frame - LastEnergySpentFrame < UnitType.EnergyRestoreDelay)
                return;
            _energyPoints += Time.fixedDeltaTime * UnitType.EnergyPointsPerSecond;
            _energyPoints = Mathf.Min(_energyPoints, MaxEnergyPoints);
        }
    }
}