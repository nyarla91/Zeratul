using Gameplay.Data.Units;
using Saving.Data.Units;

namespace Gameplay.Units
{
    public class UnitComponent
    {
        protected Unit Unit { get; private set; }
        
        public UnitType UnitType => Unit.Type;
        
        public UnitComponent(Unit unit)
        {
            Unit = unit;
        }

        public virtual IUnitSaveSystem Save() => null;

        public virtual void ReproduceFromSave(UnitSaveData saveData) { }
    }
}