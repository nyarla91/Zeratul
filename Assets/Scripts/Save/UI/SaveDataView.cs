using System;
using Save.Data;
using TMPro;
using UnityEngine;

namespace Save.UI
{
    public class SaveDataView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _meta;

        public SaveData SaveData { get; private set; }
        
        public event Action<SaveData> LoadRequested;
        public event Action<SaveData> DeletionRequested;

        public void Set(SaveData saveData)
        {
            SaveData = saveData;
            
            _name.text = saveData.filename;
            _meta.text = $"{saveData.saveTime:dd/MM/yyyy HH:mm:ss} | Mission {saveData.scenarioId} | {saveData.gameVersion}";
        }

        public void RequestLoad() => LoadRequested?.Invoke(SaveData);
        
        public void RequestDeletion() => DeletionRequested?.Invoke(SaveData);
    }
}