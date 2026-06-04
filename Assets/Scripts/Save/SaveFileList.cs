using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Save.Data;
using Zenject;

namespace Save
{
    public class SaveFileList
    {
        private readonly ISaveFileReadService _readService;
        
        private UniTask _refreshTask;
        private Dictionary<string, SaveData> _saves = new();
        
        public SaveData[] Saves => _saves.Values
            .OrderBy(s => (DateTime.Now - s.saveTime).TotalMinutes)
            .ToArray();

        public event Action RefreshStarted;
        public event Action RefreshFinished;
        

        [Inject]
        public SaveFileList(ISaveFileReadService readService)
        {
            _readService = readService;
            _readService.SaveWritten += AddSave;
            _readService.SaveDeleted += RemoveSave;
            Refresh();
        }

        private void AddSave(SaveData saveData)
        {
            _saves[saveData.filename] = saveData;
            RefreshFinished?.Invoke();
        }

        private void RemoveSave(string filename)
        {
            _saves.Remove(filename);
            RefreshFinished?.Invoke();
        }

        public void Refresh()
        {
            if (_refreshTask.Status == UniTaskStatus.Pending)
                return;
            _refreshTask = RefreshAsync();
        }

        private async UniTask RefreshAsync()
        {
            RefreshStarted?.Invoke();
            SaveData[] result = await _readService.ReadAll();
            _saves = result
                .Where(s => s.IsValid())
                .ToDictionary(s => s.filename);
            RefreshFinished?.Invoke();
        }
    }
}