using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Saving.Data.Units
{
    [Serializable]
    public class UnitDirectionSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "direction";
        public string SaveKey => "direction";

        [JsonProperty] public float LookAngle { get; }

        public UnitDirectionSaveSystem(float lookAngle)
        {
            LookAngle = lookAngle;
        }
    }
}