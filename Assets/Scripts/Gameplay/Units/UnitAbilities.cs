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
        private readonly Dictionary<AbilityType, Ability> _abilities = new();
        
        [Inject] private IPauseRead PauseRead { get; set; }
        
        public void Init(UnitType unitType)
        {
            AbilityOrder[] abilityOrders = unitType.AvailableOrders.OfType<AbilityOrder>().ToArray();
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