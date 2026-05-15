using System;

namespace Save.Data.Units
{
    [Serializable]
    public class UnitDirectionSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "direction";
        public string SaveKey => LoadKey;

        public float lookAngle;

        public UnitDirectionSaveSystem() { }
        
        public UnitDirectionSaveSystem(float lookAngle)
        {
            this.lookAngle = lookAngle;
        }
    }
}