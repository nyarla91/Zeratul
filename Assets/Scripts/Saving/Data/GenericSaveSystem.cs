using Newtonsoft.Json;

namespace Saving.Data
{
    public class GenericSaveSystem : ISaveSystem
    {
        public static string LoadKey => "generic";
        public string SaveKey => "generic";

        [JsonProperty] private int _scenarioId;

        public int ScenarioId => _scenarioId;

        public GenericSaveSystem(int scenarioId)
        {
            _scenarioId = scenarioId;
        }
    }
}