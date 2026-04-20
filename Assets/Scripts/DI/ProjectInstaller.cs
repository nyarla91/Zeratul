using Gameplay.Data.Configs;
using GameState;
using UnityEngine;
using Zenject;

namespace DI
{
    public class ProjectInstaller : MonoInstaller
    { 
        [SerializeField] private GameObject _sceneLoaderPrefab;
        [SerializeField] private TutorialRegistry _tutorialRegistry;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromComponentInNewPrefab(_sceneLoaderPrefab).AsSingle().NonLazy();
            Container.Bind<ScenarioSession>().AsSingle().NonLazy();
            Container.BindInstance(_tutorialRegistry).AsSingle().NonLazy();
        }
    }
}