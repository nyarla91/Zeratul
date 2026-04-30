using System.Collections.Generic;
using System.Linq;
using Gameplay.Player;
using Saving;
using Saving.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Saving
{
    public class GameplaySaver : MonoBehaviour
    {
        [Inject] private PlayerInput PlayerInput { get; set; }
        [Inject] private ISaveFileSaveService SaveService { get; set; }
        
        private readonly List<ISavingSystem> _systems = new();

        public void RegisterSystem(ISavingSystem system) => _systems.Add(system);

        private void Awake()
        {
            PlayerInput.QuickSave.Performed += SaveAndWrite;
        }

        private void SaveAndWrite()
        {
            SaveData data = SaveGameplayData();
            SaveService.Write(data, "bibaboba");
        }

        private SaveData SaveGameplayData()
        {
            ISaveSystem[] systems = _systems.Select(s => s.Save()).ToArray();
            return new SaveData(systems);
        }
    }
}