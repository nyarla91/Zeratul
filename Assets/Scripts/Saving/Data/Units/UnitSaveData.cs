using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Data.Units
{
    [Serializable]
    public struct UnitSaveData
    {
        [JsonProperty] private Dictionary<string, string> _systems;
        [JsonProperty] public int Id { get; }
        [JsonProperty] public string UnitType { get; }
        [JsonProperty] public Vector2 Position { get; }

        public UnitSaveData(IUnitSaveSystem[] systems, int id, string unitType, Vector2 position)
        {
            Id = id;
            UnitType = unitType;
            Position = position;
            _systems = systems.NoNull().ToDictionary(s => s.SaveKey, JsonConvert.SerializeObject);
        }

        public TSystem Get<TSystem>(string key) where TSystem : IUnitSaveSystem
        {
            string json = _systems[key];
            return JsonConvert.DeserializeObject<TSystem>(json);
        }
    }
}