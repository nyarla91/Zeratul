using System;
using Saving.Data;
using Zenject;

namespace GameState
{
    public class ScenarioSession
    {
        private readonly ScenarioRegistry _scenarioRegistry;
        
        public int CurrentId { get; private set; }
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
            GenericSaveSystem generic = data.Get<GenericSaveSystem>(GenericSaveSystem.LoadKey);
            Set(generic.ScenarioId);
        }
        
        public void Set(int scenarioId)
        {
            if (Current && Current.LoadedPrefab)
                throw new InvalidOperationException($"Unload previous Scenario before setting a new one");
            CurrentId = scenarioId;
            Current = _scenarioRegistry.Get(scenarioId);
        }
    }
}