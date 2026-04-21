using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameState
{
    [CreateAssetMenu(menuName = "Scenario Data", order = 0)]
    public class ScenarioData : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject _prefab;
        [SerializeField] private int _abilitiesAvailable;

        public GameObject LoadedPrefab { get; private set; }
        public int AbilitiesAvailable => _abilitiesAvailable;

        public async UniTask<GameObject> LoadPrefab()
        {
            AsyncOperationHandle<GameObject> handle = _prefab.LoadAssetAsync<GameObject>();
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new FileLoadException($"Couldn't load {_prefab}");
            LoadedPrefab = handle.Result;
            return handle.Result;
        }

        public void UnloadPrefab()
        {
            _prefab.ReleaseAsset();
            LoadedPrefab = null;
        }
    }
}