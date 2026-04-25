using Gameplay.Data.Configs;
using GameState;
using Saving;
using UnityEngine;
using Zenject;

namespace DI
{
    public class ProjectInstaller : MonoInstaller
    { 
        [SerializeField] private GameObject _sceneLoaderPrefab;
        [SerializeField] private ScenarioRegistry _scenarioRegistry;
        [SerializeField] private TutorialRegistry _tutorialRegistry;

        public override void InstallBindings()
        {
            Container.BindInstance(_scenarioRegistry).AsSingle().NonLazy();
            Container.BindInstance(_tutorialRegistry).AsSingle().NonLazy();
            Container.Bind<SceneLoader>().FromComponentInNewPrefab(_sceneLoaderPrefab).AsSingle().NonLazy();
            Container.Bind<ScenarioSession>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SaveFileIO>().AsSingle().NonLazy();
        }
    }
}