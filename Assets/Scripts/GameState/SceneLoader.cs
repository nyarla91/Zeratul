using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameState
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference _loadingScene;
        [SerializeField] private AssetReference _mainMenuScene;
        [SerializeField] private AssetReference _gameplayScene;

        private AsyncOperationHandle<SceneInstance>? _currentSceneHandle;
        
        public bool IsLoading { get; private set; }
        
        [Inject] private LoadingScreen LoadingScreen { get; set; }

        public void LoadGameplay(Func<UniTask> additionalLoading = null) => LoadScene(_gameplayScene, additionalLoading);
        
        public void LoadMainScene(Func<UniTask> additionalLoading = null) => LoadScene(_mainMenuScene, additionalLoading);
        
        private async UniTask LoadScene(AssetReference scene, Func<UniTask> additionalLoading)
        {
            if (IsLoading)
                return;

            IsLoading = true;

            await LoadingScreen.Show();

            if (_currentSceneHandle.HasValue)
            {
                await Addressables.UnloadSceneAsync(_currentSceneHandle.Value);
            }

            AsyncOperationHandle<SceneInstance>? loadingSceneHandle = _loadingScene.LoadSceneAsync();

            await loadingSceneHandle.Value.Task;
            
            if (additionalLoading != null)
                await additionalLoading.Invoke();

            _currentSceneHandle = scene.LoadSceneAsync(LoadSceneMode.Single, false);
            await _currentSceneHandle.Value.Task;
            
            
            await Addressables.UnloadSceneAsync(loadingSceneHandle.Value);
            await _currentSceneHandle.Value.Result.ActivateAsync().ToUniTask();
            
            SceneBootstrap bootstrap = FindAnyObjectByType<SceneBootstrap>();
            if (bootstrap)
            {
                await bootstrap.Initialize();
            }
            
            await LoadingScreen.Hide();

            IsLoading = false;
        }
    }
}