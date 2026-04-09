using Scenes;
using UnityEngine;
using Zenject;

namespace DI
{
    public class ProjectInstaller : MonoInstaller
    { 
        [SerializeField] private GameObject _sceneLoaderPrefab;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromComponentInNewPrefab(_sceneLoaderPrefab).AsSingle();
        }
    }
}