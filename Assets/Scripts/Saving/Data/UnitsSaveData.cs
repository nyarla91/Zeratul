using System;
using Newtonsoft.Json;
using Saving.Data.Units;
using UnityEngine;

namespace Saving.Data
{
    [Serializable]
    public class UnitsSaveData : ISaveSystem
    {
        public static string LoadKey => "units";
        public string SaveKey => "units";
        
        [JsonProperty] private UnitSaveData[] _units;

        public UnitSaveData[] Units => _units;

        public UnitsSaveData(UnitSaveData[] units)
        {
            _units = units;
        }
    }
}