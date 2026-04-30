using System.Linq;
using Gameplay.Units;
using Saving.Data;
using Saving.Data.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Saving
{
    public class UnitsSavingSystem : SavingSystem<UnitsSaveData>
    {
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        
        public override void ReproduceFromSaveData(UnitsSaveData payload)
        {
            
        }

        public override ISaveSystem Save()
        {
            UnitSaveData[] units =UnitPool.Units.Select(u => u.Save()).ToArray();
            return new UnitsSaveData(units);
        }
    }
}