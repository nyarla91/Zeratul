using System;
using GameState;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private int _scenarioId;
        
        [Inject] private GameFlowController GameFlowController { get; set; }

        public void Play()
        {
            GameFlowController.StartScenario(_scenarioId);
        }
    }
}