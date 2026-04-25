using UnityEngine;

namespace GameState
{
    [CreateAssetMenu(menuName = "Scenario Registry", order = 0)]
    public class ScenarioRegistry : ScriptableObject
    {
        [SerializeField] private ScenarioData[] _scenarios;

        public ScenarioData Get(int id) => _scenarios[id];
    }
}