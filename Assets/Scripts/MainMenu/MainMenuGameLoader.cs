using GameState;
using Save.UI;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MainMenuGameLoader : MonoBehaviour
    {
        [SerializeField] private SaveDataViewList _viewList;
        
        [Inject] private GameFlowController GameFlowController { get; set; }

        private void Awake()
        {
            _viewList.LoadRequested += GameFlowController.StartScenarioFromSaveData;
        }
    }
}