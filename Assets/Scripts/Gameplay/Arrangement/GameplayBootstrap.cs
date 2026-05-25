using System;
using Gameplay.Arrangement.Saving;
using Gameplay.Map;
using GameState;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement
{
    public class GameplayBootstrap : MonoBehaviour
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        [Inject] private ScenarioLifetime ScenarioLifetime { get; set; }
        [Inject] private GameplaySaveLoad GameplaySaveLoad { get; set; }
        [Inject] private NodeMap NodeMap { get; set; }
        
        private void Start()
        {
            ScenarioLifetime.Instantiate();
            if (ScenarioSession.SaveData != null)
            {
                GameplaySaveLoad.ReproduceFromSaveData(ScenarioSession.SaveData);
            }
            NodeMap.Init();
        }
    }
}