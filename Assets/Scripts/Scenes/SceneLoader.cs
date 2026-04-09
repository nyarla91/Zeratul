using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference _loadingScene;
        [SerializeField] private AssetReference _mainMenuScene;
        [SerializeField] private AssetReference _gameplayScene;

        private AsyncOperationHandle<SceneInstance>? _currentSceneHandle;
        
        public bool IsLoading { get; private set; }

        public void LoadGameplay() => LoadScene(_gameplayScene);
        
        public void LoadMainScene() => LoadScene(_mainMenuScene);

        /*private async UniTask LoadScene(AssetReference scene)
        {
            if (IsLoading)
                return;
            IsLoading = true;
            await _loadingScene.LoadSceneAsync().Task;
            await scene.LoadSceneAsync().Task;
            scene.ReleaseAsset();
            IsLoading = false;
        }*/

        private async UniTask LoadScene(AssetReference scene)
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


            _currentSceneHandle = scene.LoadSceneAsync();

            await _currentSceneHandle.Value.Task;

            await Addressables.UnloadSceneAsync(loadingSceneHandle.Value);

            IsLoading = false;
        }
    }
}