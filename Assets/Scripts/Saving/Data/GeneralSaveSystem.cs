using System;
using Newtonsoft.Json;

namespace Saving.Data
{
    [Serializable]
    public class GeneralSaveSystem : ISaveSystem
    {
        public static string LoadKey => "general";
        public string SaveKey => LoadKey;

        [JsonProperty] public int scenarioId;

        public GeneralSaveSystem() { }
        
        public GeneralSaveSystem(int scenarioId)
        {
            this.scenarioId = scenarioId;
        }
    }
}