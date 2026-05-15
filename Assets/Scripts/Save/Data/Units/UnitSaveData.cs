using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Data.Units
{
    [Serializable]
    public class UnitSaveData
    {
        [JsonProperty] private Dictionary<string, string> _systems;
          
        public int id;
        public string unitType;
        public SerializableVector2 position;

        public UnitSaveData() { }
        
        public UnitSaveData(IUnitSaveSystem[] systems, int id, string unitType, Vector2 position)
        {
            _systems = systems.ClearNull().ToDictionary(s => s.SaveKey, JsonConvert.SerializeObject);
            this.id = id;
            this.unitType = unitType;
            this.position = SerializableVector2.FromVector2(position);
        }

        public TSystem Get<TSystem>(string key) where TSystem : IUnitSaveSystem
        {
            string json = _systems[key];
            return JsonConvert.DeserializeObject<TSystem>(json);
        }
    }                                              
}