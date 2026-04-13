using System;
using GameState;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private AssetReferenceT<ScenarioData> _scenarioReference;

        private ScenarioData _scenario;
        
        [Inject] private GameFlowController GameFlowController { get; set; }

        private async void Awake()
        {
            _scenario = await _scenarioReference.LoadAssetAsync<ScenarioData>().Task;
        }

        public void Play()
        {
            if (_scenario != null)
                GameFlowController.StartScenario(_scenario);
        }
    }
}