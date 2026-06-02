using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Save.Data
{
    [Serializable]
    public class SaveData
    {
        public string filename;
        public bool quick;
        public DateTime saveTime;
        public string gameVersion;
        public int scenarioId;
        
        [JsonProperty] private Dictionary<string, string> _systems;

        public SaveData() { }
        
        public SaveData(ISaveSystem[] systems, DateTime saveTime, string gameVersion, int scenarioId)
        {
            this.saveTime = saveTime;
            this.gameVersion = gameVersion;
            this.scenarioId = scenarioId;
            _systems = systems
                .Where(s => s != null)
                .ToDictionary(s => s.SaveKey, JsonConvert.SerializeObject);
        }

        public TSystem Get<TSystem>(string key) where TSystem : ISaveSystem
        {
            string json = _systems[key];
            return JsonConvert.DeserializeObject<TSystem>(json);
        }

        public bool IsValid()
        {
            try
            {
                return Get<GeneralSaveSystem>(GeneralSaveSystem.LoadKey) != null
                       && Get<EntitiesSaveSystem>(EntitiesSaveSystem.LoadKey) != null
                       && Get<UnitsSaveSystem>(UnitsSaveSystem.LoadKey) != null
                       && Get<MapSaveSystem>(MapSaveSystem.LoadKey) != null
                       && Get<ControlSaveSystem>(ControlSaveSystem.LoadKey) != null
                       && Get<SchemeSaveSystem>(SchemeSaveSystem.LoadKey) != null;
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError($"SaveData corrupted {e}");
                return false;
            }
        }
    }
}