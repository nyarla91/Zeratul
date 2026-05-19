using System;
using GameState;
using Save;
using Save.Data;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class ContinueButton : MonoBehaviour
    {
        private SaveData _saveData;
        
        [Inject] private ISaveFileLoadService LoadService { get; set; }
        [Inject] private GameFlowController GameFlowController { get; set; }

        private async void Awake()
        {
            _saveData = await LoadService.Read("bibaboba");
            if (_saveData == null)
                Destroy(gameObject);
        }

        public void Continue()
        {
            GameFlowController.StartScenarioFromSaveData(_saveData);
        }
    }
}