using Extentions;

namespace Save.Data.Units
{
    public class UnitAllianceSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "alliance";
        public string SaveKey => LoadKey;

        public Owner initialOwner;

        public UnitAllianceSaveSystem() { }
        
        public UnitAllianceSaveSystem(Owner initialOwner)
        {
            this.initialOwner = initialOwner;
        }
    }
}