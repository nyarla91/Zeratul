using System;
using System.IO;
using Extentions;
using Gameplay.Units;
using GameState;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement
{
    public class ScenarioLifetime : MonoBehaviour
    {
        [Inject] private ContainerInstantiator ContainerInstantiator { get; set; }
        [Inject] private GameFlowController GameFlowController { get; set; }
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        
        public void Instantiate()
        {
            ScenarioData scenario = ScenarioSession.Current;
            if ( ! scenario)
                throw new NullReferenceException("No scenario is set");
            
            GameObject prefab = scenario.LoadedPrefab;
            if ( ! prefab)
                throw new FileLoadException($"{ScenarioSession.Current} prefab is not loaded");
            
            ScenarioInstantiation instantiation = ContainerInstantiator.Instantiate<ScenarioInstantiation>(prefab, Vector3.zero);

            bool spawnUnits = ScenarioSession.SaveData == null;
            foreach (UnitSpawnPoint spawnPoint in instantiation.UnitSpawnPoints)
            {
                if (spawnUnits)
                    UnitSpawner.Spawn(spawnPoint.transform.position, spawnPoint.UnitType, -1, spawnPoint.SpawnInfo);
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