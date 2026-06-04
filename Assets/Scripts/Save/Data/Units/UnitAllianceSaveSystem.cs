using _Core;

namespace Save.Data.Units
{
    public class UnitAllianceSaveSystem : IUnitSaveSystem
    {
        public static string LoadKey => "alliance";
        public string SaveKey => LoadKey;

        public Owner initialOwner;
        public Owner currentOwner;
        
        public UnitAllianceSaveSystem(Owner initialOwner, Owner currentOwner)
        {
            this.initialOwner = initialOwner;
            this.currentOwner = currentOwner;
        }
    }
}