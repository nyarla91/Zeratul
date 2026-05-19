using System.Collections.Generic;
using Gameplay.Units;
using Zenject;

namespace Gameplay.Schemes.Values.UnitGroup
{
    public class ValueUnitGroupAllUnits : SchemeValue<HashSet<Unit>>
    {
        [Inject] private UnitPool UnitPool { get; set; }

        public override HashSet<Unit> Value => UnitPool.Units;

        private void OnValidate()
        {
            gameObject.name = "All units";
        }
    }
}