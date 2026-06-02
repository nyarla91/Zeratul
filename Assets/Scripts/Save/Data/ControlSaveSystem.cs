using System.Collections.Generic;
using UnityEngine;

namespace Save.Data
{
    public class ControlSaveSystem : ISaveSystem
    {
        public static string LoadKey => "control";
        public string SaveKey => LoadKey;

        public int controlReserve;

        public HashSet<int> controlledUnits;

        public ControlSaveSystem(int controlReserve, HashSet<int> controlledUnits)
        {
            this.controlReserve = controlReserve;
            this.controlledUnits = controlledUnits;
        }

        public bool IsValid()
        {
            return controlledUnits != null;
        }
    }
}