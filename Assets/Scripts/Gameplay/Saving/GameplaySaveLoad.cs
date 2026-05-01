using System.Collections.Generic;
using System.Linq;
using Gameplay.Player;
using GameState;
using Saving;
using Saving.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Saving
{
    public class GameplaySaveLoad : MonoBehaviour
    {
        [Inject] private PlayerInput PlayerInput { get; set; }
        [Inject] private GameFlowController GameFlowController { get; set; }
        [Inject] private ISaveFileSaveService SaveService { get; set; }
        [Inject] private ISaveFileLoadService LoadService { get; set; }
        
        private readonly List<ISavingSystem> _systems = new();

        public void RegisterSystem(ISavingSystem system) => _systems.Add(system);

        public void ReproduceFromSaveData(SaveData saveData)
        {
            foreach (ISavingSystem savingSystem in _systems)
            {
                savingSystem.ReproduceFromSaveData(saveData);
            }
        }
        
        private void Awake()
        {
            PlayerInput.QuickSave.Performed += SaveAndWrite;
            PlayerInput.QuickLoad.Performed += Load;
        }

        private void SaveAndWrite()
        {
            SaveData data = SaveGameplayData();
            SaveService.Write(data, "bibaboba");
        }

        private async void Load()
        {
            SaveData saveData = await LoadService.Read("bibaboba");
            GameFlowController.StartScenarioFromSaveData(saveData);
        }

        private SaveData SaveGameplayData()
        {
            ISaveSystem[] systems = _systems.Select(s => s.Save()).ToArray();
            return new SaveData(systems);
        }
    }
}