using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Gameplay.Data.Units;
using Gameplay.Player;
using Gameplay.Units;
using Save.Data;
using Save.Data.Units;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class UnitsSavingSystem : SavingSystem<UnitsSaveSystem>
    {
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private UnitSpawner UnitSpawner { get; set; }
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        [Inject] private GameDataRegistry GameDataRegistry { get; set; }

        protected override string LoadKey => UnitsSaveSystem.LoadKey;

        public override void ReproduceFromSaveData(UnitsSaveSystem payload)
        {
            UnitSpawner.ReproduceFromSaveData(payload);
            foreach (UnitSaveData unitSaveData in payload.units)
            {
                UnitType unitType = GameDataRegistry.Get<UnitType>(unitSaveData.unitType);
                Unit unit = UnitSpawner.Spawn(unitSaveData.position.ToVector2(), unitType, unitSaveData.id);
                unit.ReproduceFromSave(unitSaveData);
            }
            
            HashSet<Unit> selection = payload.selection.Select(s => UnitPool.GetUnitById(s)).ToHashSet();
            PlayerSelection.SelectUnits(selection.ToArray());
        }

        public override ISaveSystem Save()
        {
            UnitSaveData[] units = UnitPool.Units.Select(u => u.Save()).ToArray();
            HashSet<int> selection = PlayerSelection.SelectedUnits.Select(u => u.Id).ToHashSet();
            return new UnitsSaveSystem(units, UnitSpawner.NextId, selection);
        }
    }
}