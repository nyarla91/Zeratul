using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference _loadingScene;
        [SerializeField] private AssetReference _mainMenuScene;
        [SerializeField] private AssetReference _gameplayScene;

        private AsyncOperationHandle<SceneInstance>? _currentSceneHandle;
        
        public bool IsLoading { get; private set; }

        public void LoadGameplay(UniTask additionalLoading = default) => LoadScene(_gameplayScene, additionalLoading);
        
        public void LoadMainScene(UniTask additionalLoading = default) => LoadScene(_mainMenuScene, additionalLoading);

        private async UniTask LoadScene(AssetReference scene, UniTask additionalLoading)
        {
            if (IsLoading)
                return;

            IsLoading = true;

            if (_currentSceneHandle.HasValue)
            {
                await Addressables.UnloadSceneAsync(_currentSceneHandle.Value);
            }

            AsyncOperationHandle<SceneInstance>? loadingSceneHandle = _loadingScene.LoadSceneAsync();

            await loadingSceneHandle.Value.Task;

            Debug.Log($"Loading {scene}");
            _currentSceneHandle = scene.LoadSceneAsync(LoadSceneMode.Single, false);
            await _currentSceneHandle.Value.Task;
            Debug.Log($"{scene} loaded");
            
            await additionalLoading;
            Debug.Log($"Awaiting {additionalLoading}");
            await Addressables.UnloadSceneAsync(loadingSceneHandle.Value);
            Debug.Log($"Activating {scene}");
            await _currentSceneHandle.Value.Result.ActivateAsync().ToUniTask();
            Debug.Log($"{scene} activated");

            IsLoading = false;
        }
    }
}