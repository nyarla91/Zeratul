using System.Collections.Generic;
using System.Linq;
using _Core;
using Gameplay.Data;

namespace Gameplay.Upgrades
{
    public class UpgradeStorage
    {
        private Dictionary<Owner, List<Upgrade>> _upgrades = new ();

        public Dictionary<Owner, List<Upgrade>> Upgrades =>
            _upgrades.ToDictionary(p => p.Key, p => p.Value);

        public void ResearchUpgrade(Owner owner, Upgrade upgrade)
        {
            if ( ! _upgrades.ContainsKey(owner))
                _upgrades.Add(owner, new List<Upgrade>());
            else if (_upgrades[owner].Contains(upgrade))
                return;
            _upgrades[owner].Add(upgrade);
        }

        public bool IsUpgradeResearched(Owner owner, Upgrade upgrade)
        {
            return _upgrades.TryGetValue(owner, out List<Upgrade> upgrades) && upgrades.Contains(upgrade);
        }
    }
}