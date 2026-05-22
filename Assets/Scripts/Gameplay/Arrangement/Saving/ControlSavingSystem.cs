using System.Collections.Generic;
using System.Linq;
using Gameplay.Player;
using Gameplay.Units;
using Save.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class ControlSavingSystem : SavingSystem<ControlSaveSystem>
    {
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }

        protected override string LoadKey => ControlSaveSystem.LoadKey;

        public override void ReproduceFromSaveData(ControlSaveSystem payload)
        {
            PlayerControlResources.ReproduceFromSaveData(payload, UnitPool);
        }

        public override ISaveSystem Save()
        {
            int controlReserve = PlayerControlResources.Reserve;
            HashSet<int> controlledUnits = PlayerControlResources.ControlledUnits.Select(u => u.Id).ToHashSet();
            return new ControlSaveSystem(controlReserve, controlledUnits);
        }
    }
}