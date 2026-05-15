using System;
using Save.Data;
using Zenject;

namespace GameState
{
    public class ScenarioSession
    {
        private readonly ScenarioRegistry _scenarioRegistry;

        public int CurrentId { get; private set; } = -1;
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
            GeneralSaveSystem general = data.Get<GeneralSaveSystem>(GeneralSaveSystem.LoadKey);
            Set(general.scenarioId);
        }
        
        public void Set(int scenarioId)
        {
            if (scenarioId == CurrentId)
                return;
            if (Current && Current.LoadedPrefab)
                Current.UnloadPrefab();
            CurrentId = scenarioId;
            Current = _scenarioRegistry.Get(scenarioId);
        }
    }
}