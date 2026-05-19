using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Values.UnitGroup
{
    public class ValueUnitGroupAllUnits : SchemeValue<HashSet<Unit>>
    {
        [SerializeField] private UnitValidatorGroup _validator;

        [Inject] private UnitPool UnitPool { get; set; }
        
        public override HashSet<Unit> Value => UnitPool.Units
            .Where(u => _validator.IsValid(u, u))
            .ToHashSet();

        private void OnValidate()
        {
            gameObject.name = $"All units {_validator}";
        }
    }
}