using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Values.UnitGroup
{
    public class ValueUnitGroupValidUnits : SchemeValue<HashSet<Unit>>
    {
        [SerializeField] private SchemeValue<HashSet<Unit>> _unitGroup;
        [SerializeField] private UnitValidatorGroup _validator;
        
        public override HashSet<Unit> Value => _unitGroup.Value
            .Where(u => _validator.IsValid(u, u))
            .ToHashSet();

        private void OnValidate()
        {
            gameObject.name = $"Valid {_validator} units from ({_unitGroup?.gameObject.name})";
        }
    }
}