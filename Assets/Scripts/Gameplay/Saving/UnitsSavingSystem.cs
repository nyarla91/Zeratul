using System.Linq;
using Gameplay.Data;
using Gameplay.Data.Units;
using Gameplay.Units;
using Saving.Data;
using Saving.Data.Units;
using Zenject;

namespace Gameplay.Saving
{
    public class UnitsSavingSystem : SavingSystem<UnitsSaveData>
    {
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        [Inject] private GameDataRegistry GameDataRegistry { get; set; }

        protected override string LoadKey => UnitsSaveData.LoadKey;

        public override void ReproduceFromSaveData(UnitsSaveData payload)
        {
            UnitSpawner.ReproduceFromSaveData(payload);
            foreach (UnitSaveData unitSaveData in payload.units)
            {
                UnitType unitType = GameDataRegistry.Get<UnitType>(unitSaveData.unitType);
                Unit unit = UnitSpawner.Spawn(unitSaveData.position.ToVector2(), unitType, unitSaveData.id);
                unit.ReproduceFromSave(unitSaveData);
            }
        }

        public override ISaveSystem Save()
        {
            UnitSaveData[] units = UnitPool.Units.Select(u => u.Save()).ToArray();
            return new UnitsSaveData(units, UnitSpawner.NextId);
        }
    }
}