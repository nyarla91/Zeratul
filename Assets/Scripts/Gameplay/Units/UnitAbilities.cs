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
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitAbilities : UnitComponent
    {
        private Timer _energyRestorationTimer;
        private float _energyRestorationRemain;
        
        public int EnergyPoints { get; private set; }

        public int MaxEnergyPoints => UnitType.MaxEnergyPoints; 
        
        public bool HasEnergyPoints => MaxEnergyPoints > 0;
        public float EnergyPercent => HasEnergyPoints ? (float) EnergyPoints / MaxEnergyPoints : 1;
        
        private readonly Dictionary<AbilityType, Ability> _abilities = new();
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        
        public void Init(UnitType unitType)
        {
            EnergyPoints = MaxEnergyPoints;
            
            if (unitType.EnergyRestoreDelay > 0)
                _energyRestorationTimer = new Timer(this, unitType.EnergyRestoreDelay, TacticalPause);
            
            AbilityOrder[] abilityOrders = unitType.AvailableOrders.OfType<AbilityOrder>().ToArray();
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
            ability.Caster.Abilities.SpendEnergy(abilityType.EnergyCost);
            return true;
        }

        public void SpendEnergy(int energy)
        {
            if (energy <= 0)
                return;
            if (energy > EnergyPoints)
                throw new ArgumentOutOfRangeException();
            EnergyPoints -= energy;
        }
        
        public Ability GetAbility(AbilityType abilityType) =>  _abilities[abilityType];

        private void FixedUpdate()
        {
            if (_energyRestorationTimer == null || _energyRestorationTimer.IsOn || TacticalPause.IsPaused)
                return;
            _energyRestorationRemain += Time.fixedDeltaTime * UnitType.EnergyPointsPerSecond;
            _energyRestorationRemain = Mathf.Min(_energyRestorationRemain, MaxEnergyPoints - EnergyPoints);
            EnergyPoints += (int) _energyRestorationRemain;
            _energyRestorationRemain %= 1;
        }
    }
}