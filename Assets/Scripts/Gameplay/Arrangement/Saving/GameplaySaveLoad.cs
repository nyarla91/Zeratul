using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Player;
using GameState;
using Save;
using Save.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class GameplaySaveLoad : MonoBehaviour
    {
        [Inject] private PlayerInput PlayerInput { get; set; }
        [Inject] private GameFlowController GameFlowController { get; set; }
        [Inject] private ISaveFileWriteService SaveService { get; set; }
        [Inject] private ISaveFileReadService ReadService { get; set; }
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        
        private List<ISavingSystem> _systems = new();

        public void RegisterSystem(ISavingSystem system) => _systems.Add(system);

        public void ReproduceFromSaveData(SaveData saveData)
        {
            _systems = _systems.OrderBy(s => s.LoadPriority).ToList();
            foreach (ISavingSystem savingSystem in _systems)
            {
                try
                {
                    Debug.Log($"Loading {savingSystem}");
                    savingSystem.ReproduceFromSaveData(saveData);
                    Debug.Log($"Loading {savingSystem} completed");
                }
                catch (Exception e)
                {
                    Debug.LogError($"There was an error loading {savingSystem}. {e.Message}");
                    throw;
                }
            }
        }

        public SaveData SaveGameplayData()
        {
            ISaveSystem[] systems = _systems.Select(s => s.Save()).ToArray();
            DateTime saveTime = DateTime.Now;
            int id = ScenarioSession.CurrentId;
            string gameVersion = Application.version;
            return new SaveData(systems, saveTime, gameVersion, id);
        }
    }
}