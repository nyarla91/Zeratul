using System.Collections.Generic;
using _Core;
using UnityEngine;

namespace Save.Data
{
    public class UpgradeSaveSystem : ISaveSystem
    {
        public static string LoadKey => "upgrade";
        public string SaveKey => LoadKey;

        public Dictionary<Owner, List<string>> upgrades;

        public UpgradeSaveSystem() { }
        
        public UpgradeSaveSystem(Dictionary<Owner, List<string>> upgrades)
        {
            this.upgrades = upgrades;
        }
    }
}