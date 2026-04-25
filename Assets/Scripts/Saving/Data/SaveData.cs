using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Data
{
    [Serializable]
    public class SaveData
    {
        [JsonProperty] private Dictionary<string, string> _systems;

        public SaveData(ISaveSystem[] systems)
        {
            _systems = systems.ToDictionary(s => s.SaveKey, JsonConvert.SerializeObject);
        }

        public TSystem Get<TSystem>(string key) where TSystem : ISaveSystem
        {
            string json = _systems[key];
            return JsonConvert.DeserializeObject<TSystem>(json);
        }
    }
}