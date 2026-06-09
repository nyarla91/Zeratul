using _Core;
using Gameplay.Data;
using Gameplay.Upgrades;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionResearchUpgrade : SchemeAction
    {
        [SerializeField] private Owner _owner;
        [SerializeField] private Upgrade _upgrade;
        
        [Inject] private UpgradeStorage UpgradeStorage { get; set; }
        
        public override void Act()
        {
            UpgradeStorage.ResearchUpgrade(_owner, _upgrade);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Research {_upgrade?.name} for {_owner:G}";
        }
    }
}