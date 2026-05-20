using System;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionAddUnitToUnitGroup : SchemeAction
    {
        [SerializeField] private VariableUnitGroup _unitGroup;
        [SerializeField] private SchemeValue<Unit> _unit;
        
        public override void Act()
        {
            _unitGroup.AddUnit(_unit?.Value);
        }

        private void OnValidate()
        {
            gameObject.name = $"Add {_unit?.name} to {_unitGroup?.name}";
        }
    }
}