using System;
using Cysharp.Threading.Tasks;
using Extentions.Pause;
using Gameplay.Arrangement.Saving;
using Gameplay.Map;
using GameState;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement
{
    public class GameplayBootstrap : SceneBootstrap
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        [Inject] private ScenarioLifetime ScenarioLifetime { get; set; }
        [Inject] private GameplaySaveLoad GameplaySaveLoad { get; set; }
        [Inject] private NodeMap NodeMap { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        
        public override async UniTask Initialize()
        {
            GamePause.Pause(this);
            ScenarioLifetime.Instantiate();
            if (ScenarioSession.SaveData != null)
            {
                GameplaySaveLoad.ReproduceFromSaveData(ScenarioSession.SaveData);
            }
            NodeMap.Init();
            GamePause.Unpause(this);
        }
    }
}