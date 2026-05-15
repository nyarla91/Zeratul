using System;
using Newtonsoft.Json;
using Save.Data.Units;

namespace Save.Data
{
    [Serializable]
    public class UnitsSaveSystem : ISaveSystem
    {
        public static string LoadKey => "units";
        public string SaveKey => LoadKey;

        public UnitSaveData[] units;
        public int nextId;

        public UnitsSaveSystem() { }
        
        public UnitsSaveSystem(UnitSaveData[] units, int nextId)
        {
            this.units = units;
            this.nextId = nextId;
        }
    }
}