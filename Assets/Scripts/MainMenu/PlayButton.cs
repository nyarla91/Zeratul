using GameState;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private ScenarioData _scenario;
        
        [Inject] private GameFlowController GameFlowController { get; set; }

        public void Play()
        {
            GameFlowController.StartScenario(_scenario);
        }
    }
}