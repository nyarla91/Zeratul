using System;
using Newtonsoft.Json;
using Saving.Data.Units;

namespace Saving.Data
{
    [Serializable]
    public class UnitsSaveSystem : ISaveSystem
    {
        public static string LoadKey => "units";
        public string SaveKey => LoadKey;

        [JsonProperty] public UnitSaveData[] units;
        [JsonProperty] public int nextId;

        public UnitsSaveSystem() { }
        
        public UnitsSaveSystem(UnitSaveData[] units, int nextId)
        {
            this.units = units;
            this.nextId = nextId;
        }
    }
}