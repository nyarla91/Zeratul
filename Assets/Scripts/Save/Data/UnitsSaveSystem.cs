using System;
using System.Collections.Generic;
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
        public HashSet<int> selection;

        public UnitsSaveSystem() { }
        
        public UnitsSaveSystem(UnitSaveData[] units, int nextId, HashSet<int> selection)
        {
            this.units = units;
            this.nextId = nextId;
            this.selection = selection;
        }
    }
}