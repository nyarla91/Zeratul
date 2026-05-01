using System;
using Newtonsoft.Json;
using Saving.Data.Units;

namespace Saving.Data
{
    [Serializable]
    public class UnitsSaveData : ISaveSystem
    {
        public static string LoadKey => "units";
        public string SaveKey => LoadKey;

        [JsonProperty] public UnitSaveData[] units;
        [JsonProperty] public int nextId;

        public UnitsSaveData() { }
        
        public UnitsSaveData(UnitSaveData[] units, int nextId)
        {
            this.units = units;
            this.nextId = nextId;
        }
    }
}