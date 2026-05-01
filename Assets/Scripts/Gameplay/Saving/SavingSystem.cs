using System;
using Saving.Data;
using UnityEngine;

namespace Gameplay.Saving
{
    public abstract class SavingSystem<TSystem> : MonoBehaviour, ISavingSystem where TSystem : ISaveSystem
    {
        [SerializeField] private GameplaySaveLoad _saveLoad;
        
        protected abstract string LoadKey { get; }

        public void ReproduceFromSaveData(SaveData saveData) => ReproduceFromSaveData(saveData.Get<TSystem>(LoadKey));
        
        public abstract void ReproduceFromSaveData(TSystem payload);

        public abstract ISaveSystem Save();

        protected virtual void Awake()
        {
           _saveLoad.RegisterSystem(this);
        }
    }

    public interface ISavingSystem
    {
        public void ReproduceFromSaveData(SaveData payload);
        public ISaveSystem Save();
    }
}