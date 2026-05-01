using System;
using UnityEngine;

namespace Saving.Data.Units
{
    [Serializable]
    public class UnitStatusesSaveData : IUnitSaveSystem
    {
        public static string LoadKey => "statuses";
        public string SaveKey => LoadKey;

        public StatusSaveData[] statuses;
        
        public UnitStatusesSaveData(StatusSaveData[] statuses)
        {
            this.statuses = statuses;
        }
    }
}