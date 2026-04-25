using Newtonsoft.Json;

namespace Saving.Data
{
    public class SaveGenericSystem : ISaveSystem
    {
        public static string LoadKey => "generic";
        public string SaveKey => "generic";

        [JsonProperty] private int _scenarioId;

        public int ScenarioId => _scenarioId;
    }
}