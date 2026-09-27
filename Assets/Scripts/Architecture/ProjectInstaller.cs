using BGM;
using Gameplay.Data.Configs;
using GameState;
using Save;
using Settings;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Architecture
{
    public class ProjectInstaller : MonoInstaller
    { 
        [SerializeField] private GameObject _loadingScreenPrefab;
        [SerializeField] private GameObject _sceneLoaderPrefab;
        [SerializeField] private GameObject _bgmPrefab;
        [SerializeField] private ScenarioRegistry _scenarioRegistry;
        [SerializeField] private TutorialRegistry _tutorialRegistry;
        [SerializeField] private AudioMixer _audioMixer;

        public override void InstallBindings()
        {
            Container.BindInstance(_scenarioRegistry).AsSingle().NonLazy();
            Container.BindInstance(_tutorialRegistry).AsSingle().NonLazy();
            Container.Bind<LoadingScreen>().FromComponentInNewPrefab(_loadingScreenPrefab).AsSingle().NonLazy();
            Container.Bind<SceneLoader>().FromComponentInNewPrefab(_sceneLoaderPrefab).AsSingle().NonLazy();
            Container.Bind<BgmCycle>().FromComponentInNewPrefab(_bgmPrefab).AsSingle().NonLazy();
            Container.Bind<ScenarioSession>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SaveFileIO>().AsSingle().NonLazy();
            Container.Bind<SaveFileList>().AsSingle().NonLazy();
            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle().NonLazy();
            Container.Bind<SettingsApplier>().AsSingle().NonLazy();
        }
    }
}