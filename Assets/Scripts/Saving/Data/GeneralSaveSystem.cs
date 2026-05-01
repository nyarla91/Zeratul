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
        [JsonProperty] public int gameTimeFrame;
        [JsonProperty] public int gameTimeUnpausedFrame;

        public GeneralSaveSystem() { }
        
        public GeneralSaveSystem(int scenarioId, int gameTimeFrame, int gameTimeUnpausedFrame)
        {
            this.scenarioId = scenarioId;
            this.gameTimeFrame = gameTimeFrame;
            this.gameTimeUnpausedFrame = gameTimeUnpausedFrame;
        }
    }
}