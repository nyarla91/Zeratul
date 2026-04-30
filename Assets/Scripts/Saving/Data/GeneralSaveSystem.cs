using Newtonsoft.Json;

namespace Saving.Data
{
    public class GeneralSaveSystem : ISaveSystem
    {
        public static string LoadKey => "general";
        public string SaveKey => "general";

        [JsonProperty] public int ScenarioId { get; }

        public GeneralSaveSystem(int scenarioId)
        {
            ScenarioId = scenarioId;
        }
    }
}