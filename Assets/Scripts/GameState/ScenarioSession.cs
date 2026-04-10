using System;
using UnityEngine;

namespace GameState
{
    public class ScenarioSession
    {
        public ScenarioData Current { get; private set; }

        public void Set(ScenarioData scenario)
        {
            if (Current && Current.LoadedPrefab)
                throw new InvalidOperationException($"Unload previous Scenario before setting a new one");
            Current = scenario;
        }
    }
}