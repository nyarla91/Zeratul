using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Newtonsoft.Json;
using UnityEngine;

namespace Saving.Data
{
    [Serializable]
    public class SaveData
    {
        [JsonProperty] private Dictionary<string, string> _systems;

        public SaveData() { }
        
        public SaveData(ISaveSystem[] systems)
        {
            Debug.Log(systems.Length);
            _systems = systems
                .Where(s => s != null)
                .ToDictionary(s => s.SaveKey, JsonConvert.SerializeObject);
        }

        public TSystem Get<TSystem>(string key) where TSystem : ISaveSystem
        {
            string json = _systems[key];
            return JsonConvert.DeserializeObject<TSystem>(json);
        }
    }
}