using System;
using Saving.Data;
using UnityEngine;

namespace Gameplay.Saving
{
    public abstract class SavingSystem<TSystem> : MonoBehaviour, ISavingSystem where TSystem : ISaveSystem
    {
        [SerializeField] private GameplaySaver _saver;
        
        public abstract void ReproduceSavedSystem(TSystem payload);
        
        public abstract ISaveSystem SaveSystem();

        protected virtual void Awake()
        {
           _saver.RegisterSystem(this);
        }
    }

    public interface ISavingSystem
    {
        public ISaveSystem SaveSystem();
    }
}