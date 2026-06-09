using System.Collections.Generic;
using System.Linq;
using _Core;
using Gameplay.Data;
using Gameplay.Upgrades;
using Save.Data;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class UpgradeSavingSystem : SavingSystem<UpgradeSaveSystem>
    {
        protected override string LoadKey => UpgradeSaveSystem.LoadKey;
        
        [Inject] private UpgradeStorage UpgradeStorage { get; set; }
        [Inject] private GameDataRegistry GameDataRegistry { get; set; }
        
        public override void ReproduceFromSaveData(UpgradeSaveSystem payload)
        {
            foreach (KeyValuePair<Owner, List<string>> pair in payload.upgrades)
            {
                foreach (string upgradeName in pair.Value)
                {
                    Upgrade upgrade = GameDataRegistry.Get<Upgrade>(upgradeName);
                    UpgradeStorage.ResearchUpgrade(pair.Key, upgrade);
                }
                    
            }
        }

        public override ISaveSystem Save()
        {
            Dictionary<Owner, List<string>> upgrades = UpgradeStorage.Upgrades
                .ToDictionary(p => p.Key, p => p.Value
                    .Select(u => u.name)
                    .ToList());
            
            return new UpgradeSaveSystem(upgrades);
        }
    }
}