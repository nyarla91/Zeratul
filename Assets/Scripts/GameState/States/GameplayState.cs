using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace GameState.States
{
    public class GameplayState : IGameState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly ScenarioSession _scenarioSession;

        [Inject]
        public GameplayState(SceneLoader sceneLoader, ScenarioSession scenarioSession)
        {
            _sceneLoader = sceneLoader;
            _scenarioSession = scenarioSession;
        }

        public void RestartScenario()
        {
            if ( ! _scenarioSession.Current)
                throw new NullReferenceException("No scenario is set");
            if ( ! _scenarioSession.Current.LoadedPrefab)
                throw new FileLoadException($"{_scenarioSession.Current} prefab is not loaded");
            
            _sceneLoader.LoadGameplay();
        }

        public async void Enter()
        {
            _sceneLoader.LoadGameplay(() => _scenarioSession.Current.LoadPrefab());
        }

        public void Exit()
        {
            _scenarioSession.Current.UnloadPrefab();
        }
    }
}