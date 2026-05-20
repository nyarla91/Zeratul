using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolUnitGroupContainsUnit : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<HashSet<Unit>> _unitGroup; 
        [SerializeField] private SchemeValue<Unit> _unit;

        public override bool Value => _unitGroup?.Value.Contains(_unit.Value) ?? false;
    }
}