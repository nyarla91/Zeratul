using System;
using System.Collections.Generic;
using Extentions;
using Newtonsoft.Json;

namespace Saving.Data.Units
{
    [Serializable]
    public class UnitAbilitiesSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "abilities";
        public string SaveKey => LoadKey;

        [JsonProperty] public float energyPoints;
        [JsonProperty] public int lastEnergySpentFrame;
        [JsonProperty] public Dictionary<string, int> lastCastFrameByAbilityName;

        public UnitAbilitiesSaveSystem(float energyPoints, int lastEnergySpentFrame, Dictionary<string, int> lastCastFrameByAbilityName)
        {
            this.energyPoints = energyPoints;
            this.lastEnergySpentFrame = lastEnergySpentFrame;
            this.lastCastFrameByAbilityName = lastCastFrameByAbilityName;
        }
    }
}