using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    [CreateAssetMenu(menuName = "Scenario Registry", order = 0)]
    public class ScenarioRegistry : ScriptableObject
    {
        [SerializeField] private List<ScenarioData> _scenarios;

        public ScenarioData Get(int id) => _scenarios[id];

        public int GetId(ScenarioData scenario)
        {
            if ( ! _scenarios.Contains(scenario))
                return -1;
            return _scenarios.IndexOf(scenario);
        }
    }
}