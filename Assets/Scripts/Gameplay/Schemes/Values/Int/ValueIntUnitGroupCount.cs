using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values.Int
{
    public class ValueIntUnitGroupCount : SchemeValue<int> 
    {
        [SerializeField] private SchemeValue<HashSet<Unit>> _unitGroup;

        public override int Value => _unitGroup.Value?.Count ?? 0;

        private void OnValidate()
        {
            gameObject.name = $"({_unitGroup?.name}) count";
        } 
    }
}