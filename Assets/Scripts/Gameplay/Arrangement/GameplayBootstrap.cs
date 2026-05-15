using System;
using Gameplay.Arrangement.Saving;
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
        
        private void Awake()
        {
            ScenarioLifetime.Instantiate();
            if (ScenarioSession.SaveData != null)
            {
                GameplaySaveLoad.ReproduceFromSaveData(ScenarioSession.SaveData);
            }
        }
    }
}