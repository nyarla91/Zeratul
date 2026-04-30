using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Units;
using GameState;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class ScenarioLifetime : MonoBehaviour
    {
        [Inject] private ContainerInstantiator ContainerInstantiator { get; set; }
        [Inject] private GameFlowController GameFlowController { get; set; }
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        
        private void Awake()
        {
            ScenarioData scenario = ScenarioSession.Current;
            if ( ! scenario)
                throw new NullReferenceException("No scenario is set");
            
            GameObject prefab = scenario.LoadedPrefab;
            if ( ! prefab)
                throw new FileLoadException($"{ScenarioSession.Current} prefab is not loaded");
            
            ScenarioInstantiation instantiation = ContainerInstantiator.Instantiate<ScenarioInstantiation>(prefab, Vector3.zero);

            foreach (UnitSpawnPoint spawnPoint in instantiation.UnitSpawnPoints)
            {
                UnitSpawner.Spawn(spawnPoint.SpawnInfo, spawnPoint.transform.position);
                spawnPoint.Dispose();
            }
        }
        
        public void RestartScenario()
        {
            GameFlowController.RestartScenario();
        }

        public void LeaveScenario()
        {
            GameFlowController.LeaveScenario();
        }
    }
}