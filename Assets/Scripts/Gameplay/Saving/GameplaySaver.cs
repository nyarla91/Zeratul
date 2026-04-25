using System.Collections.Generic;
using System.Linq;
using Saving.Data;
using UnityEngine;

namespace Gameplay.Saving
{
    public class GameplaySaver : MonoBehaviour
    {
        private List<ISavingSystem> _systems;

        public void RegisterSystem(ISavingSystem system) => _systems.Add(system);

        public SaveData SaveData()
        {
            ISaveSystem[] systems = _systems.Select(s => s.SaveSystem()).ToArray();
            return new SaveData(systems);
        }
    }
}