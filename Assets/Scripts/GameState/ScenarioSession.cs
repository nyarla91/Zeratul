using System;
using Saving.Data;
using Zenject;

namespace GameState
{
    public class ScenarioSession
    {
        private readonly ScenarioRegistry _scenarioRegistry;
        
        public ScenarioData Current { get; private set; }
        public SaveData SaveData { get; private set; }

        [Inject]
        public ScenarioSession(ScenarioRegistry scenarioRegistry)
        {
            _scenarioRegistry = scenarioRegistry;
        }

        public void ClearSaveData()
        {
            SaveData = null;
        }

        public void SetSaveData(SaveData data)
        {
            SaveData = data;
            SaveGenericSystem generic = data.Get<SaveGenericSystem>(SaveGenericSystem.LoadKey);
            Set(_scenarioRegistry.Get(generic.ScenarioId));
        }
        
        public void Set(ScenarioData scenario)
        {
            if (Current && Current.LoadedPrefab)
                throw new InvalidOperationException($"Unload previous Scenario before setting a new one");
            Current = scenario;
        }
    }
}