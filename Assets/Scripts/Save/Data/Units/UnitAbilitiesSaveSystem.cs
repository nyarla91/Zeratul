using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Save.Data.Units
{
    [Serializable]
    public class UnitAbilitiesSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "abilities";
        public string SaveKey => LoadKey;

        [JsonProperty] public float energyPoints;
        [JsonProperty] public int lastEnergySpentFrame;
        [JsonProperty] public Dictionary<string, AbilitySaveData> abilities;

        public UnitAbilitiesSaveSystem(float energyPoints, int lastEnergySpentFrame, Dictionary<string, AbilitySaveData> abilities)
        {
            this.energyPoints = energyPoints;
            this.lastEnergySpentFrame = lastEnergySpentFrame;
            this.abilities = abilities;
        }
    }

    [Serializable]
    public class AbilitySaveData
    {
        public int lastCastFrame;
        public int charges;

        public AbilitySaveData(int lastCastFrame, int charges)
        {
            this.lastCastFrame = lastCastFrame;
            this.charges = charges;
        }
    }
}