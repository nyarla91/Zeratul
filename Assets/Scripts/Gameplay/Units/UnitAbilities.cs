using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Abilities;
using Gameplay.Data.Effects;
using Gameplay.Data.Orders;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitAbilities : UnitComponent
    {
        private Timer _energyRestorationTimer;

        private float _energyPoints;
        public int EnergyPoints => Mathf.FloorToInt(_energyPoints);

        public int MaxEnergyPoints => UnitType.MaxEnergyPoints; 
        
        public bool HasEnergyPoints => MaxEnergyPoints > 0;
        public float EnergyPercent => HasEnergyPoints ? (float) EnergyPoints / MaxEnergyPoints : 1;
        
        private readonly Dictionary<AbilityType, Ability> _abilities = new();

        private IPauseReadonly TacticalPause { get; set; }

        public UnitAbilities(Unit unit, IPauseReadonly tacticalPause) : base(unit)
        {
            TacticalPause = tacticalPause;
            
            Observable.EveryFixedUpdate()
                .Where(_ => TacticalPause.IsUnpaused)
                .Subscribe(_ => RestoreEnergyPoints());
            
            _energyPoints = MaxEnergyPoints;
            
            if (UnitType.EnergyRestoreDelay > 0)
                _energyRestorationTimer = new Timer(UnitType.EnergyRestoreDelay, TacticalPause);
            
            AbilityOrder[] abilityOrders = UnitType.AvailableOrders.OfType<AbilityOrder>().ToArray();
            
            if (abilityOrders.Length == 0)
                return;
            
            foreach (AbilityOrder abilityOrder in abilityOrders)
            {
                _abilities.Add(abilityOrder.AbilityType, new Ability(abilityOrder.AbilityType, Unit, TacticalPause));
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
            return true;
        }

        private bool TrySpendEnergy(int energy)
        {
            if (energy <= 0)
                return true;
            if (energy > EnergyPoints)
                return false;
            _energyPoints -= energy;
            _energyRestorationTimer.Restart();
            return true;
        }
        
        public Ability GetAbility(AbilityType abilityType) =>  _abilities[abilityType];

        private void RestoreEnergyPoints()
        {
            if (_energyRestorationTimer?.IsOn ?? false)
                return;
            _energyPoints += Time.fixedDeltaTime * UnitType.EnergyPointsPerSecond;
            _energyPoints = Mathf.Min(_energyPoints, MaxEnergyPoints);
        }
    }
}