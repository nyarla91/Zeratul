using System;
using System.Linq;
using Save.Data;
using Zenject;

namespace Save
{
    public class SaveFileList
    {
        private readonly ISaveFileReadService _readService;
        
        private SaveData[] _saves = { };
        
        public SaveData[] Saves => _saves.ToArray();

        public event Action Refreshed;

        [Inject]
        public SaveFileList(ISaveFileReadService readService)
        {
            _readService = readService;
        }

        public async void Refresh()
        {
            SaveData[] result = await _readService.ReadAll();
            _saves = result
                .Where(s => s.IsValid())
                .OrderBy(s => (DateTime.Now - s.saveTime).TotalMinutes)
                .ToArray();
            Refreshed?.Invoke();
        }
    }
}