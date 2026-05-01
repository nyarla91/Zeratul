using System;

namespace Saving.Data.Units
{
    [Serializable]
    public class UnitLifeSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "life";
        public string SaveKey => LoadKey;

        public float hitPoints;
        public float shieldPoints;
        public int lastDamageDealerId;
        public int lastDamageFrame;
        
        public UnitLifeSaveSystem() { }
        
        public UnitLifeSaveSystem(float hitPoints, float shieldPoints, int lastDamageDealerId, int lastDamageFrame)
        {
            this.hitPoints = hitPoints;
            this.shieldPoints = shieldPoints;
            this.lastDamageDealerId = lastDamageDealerId;
            this.lastDamageFrame = lastDamageFrame;
        }
    }
}