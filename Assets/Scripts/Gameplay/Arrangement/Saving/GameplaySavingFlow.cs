using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Player;
using Gameplay.UI;
using GameState;
using Save;
using Save.Data;
using Save.UI;
using Settings.Localization;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class GameplaySavingFlow : MonoBehaviour
    {
        [SerializeField] private GameplaySaveLoad _saveLoad;
        [SerializeField] private SaveDataViewList[] _viewLists;
        [SerializeField] private string _quickSaveFilename;
        [SerializeField] private int _maxQuickSaves;
        [SerializeField] private string _successMessage;
        [SerializeField] private string _saveInProgressMessage;
        [SerializeField] private string _errorMessage;

        private UniTask _savingTask;
        
        [Inject] private Localizer Localizer { get; set; }
        [Inject] private ISaveFileWriteService SaveFileWriteService { get; set; }
        [Inject] private GameFlowController GameFlowController { get; set; }
        [Inject] private SaveFileList SaveFileList { get; set; }
        [Inject] private PlayerInput PlayerInput { get; set; }
        [Inject] private Message Message { get; set; }

        private void Awake()
        {
            foreach (SaveDataViewList list in _viewLists)
            {
                list.LoadRequested += GameFlowController.StartScenarioFromSaveData;
            }
            PlayerInput.QuickSave.Performed += QuickSave;
            PlayerInput.QuickLoad.Performed += QuickLoad;
        }

        public void Save(string name, bool quick)
        {
            if (_savingTask.Status == UniTaskStatus.Pending)
            {
                return;
            }
            _savingTask = SaveAsync(name, quick);
        }

        public void QuickLoad()
        {
            if (SaveFileList.Saves.Length > 0)
                GameFlowController.StartScenarioFromSaveData(SaveFileList.Saves[0]);
        }

        private async UniTask SaveAsync(string name, bool quick)
        {
            SaveData saveData = _saveLoad.SaveGameplayData();
            saveData.filename = name;
            saveData.quick = quick;
            bool successful = await SaveFileWriteService.Write(saveData);
            if (!successful)
            {
                Message.Show(Localizer.Translate(_errorMessage), MessageType.Error);
                return;
            }
            if (quick)
                DeleteExcessQuickSaves();
            Message.Show(Localizer.Translate(_successMessage), MessageType.Success);
        }

        private void DeleteExcessQuickSaves()
        {
            SaveData[] saves = SaveFileList.Saves;
            HashSet<string> forDeletion = new();
            int quickCount = 0;
            for (int i = 0; i < saves.Length; i++)
            {
                if ( ! saves[i].quick)
                    continue;
                quickCount++;
                if (quickCount > _maxQuickSaves)
                    forDeletion.Add(saves[i].filename);
            }
            foreach (string filename in forDeletion)
            {
                SaveFileWriteService.Delete(filename);
            }
        }

        private void QuickSave()
        {
            string filename = Localizer.Translate(_quickSaveFilename);
            filename += " " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
            Save(filename, true);
        }
    }
}