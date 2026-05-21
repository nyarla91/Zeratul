using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionRemoveUnitFromUnitGroup : SchemeAction
    {
        [SerializeField] private VariableUnitGroup _unitGroup;
        [SerializeField] private SchemeValue<Unit> _unit;
        
        public override void Act()
        {
            _unitGroup.RemoveUnit(_unit?.Value);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Remove {_unit?.name} from {_unitGroup?.name}";
        }
    }
}