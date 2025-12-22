using System.Collections.Generic;
using System.Linq;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Abilities;
using Gameplay.Data.Orders;
using Zenject;

namespace Gameplay.Units
{
    public class UnitAbilities : UnitComponent
    {
        public int EnergyPoints { get; private set; }

        public int MaxEnergyPoints => UnitType.Abilities.MaxEnergyPoints; 
        
        public bool HasEnergyPoints => MaxEnergyPoints > 0;
        public float EnergyPercent => HasEnergyPoints ? 1 : (float) EnergyPoints / MaxEnergyPoints;
        
        private readonly Dictionary<AbilityType, Ability> _abilities = new();
        
        [Inject] private IPauseRead PauseRead { get; set; }
        
        public void Init(UnitType unitType)
        {
            EnergyPoints = MaxEnergyPoints;
            
            AbilityOrder[] abilityOrders = unitType.Abilities.AvailableOrders.OfType<AbilityOrder>().ToArray();
            if (abilityOrders.Length == 0)
                return;
            foreach (AbilityOrder abilityOrder in abilityOrders)
            {
                _abilities.Add(abilityOrder.AbilityType, new Ability(abilityOrder.AbilityType, Composition, PauseRead));
            }
        }
        
        public Ability GetAbility(AbilityType abilityType) =>  _abilities[abilityType];
    }
}