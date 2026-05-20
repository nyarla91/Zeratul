using System.Collections.Generic;
using System.Linq;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionForeachUntInUnitGroup : SchemeAction
    {
        [SerializeField] private SchemeValue<HashSet<Unit>> _unitGroup;
        [SerializeField] private VariableUnit _out;
        [SerializeField] private SchemeAction[] _actions;
        
        public override void Act()
        {
            foreach (Unit unit in _unitGroup.Value)
            {
                _out?.Set(unit);
                foreach (SchemeAction action in _actions)
                {
                    action.Act();
                }
            }
        }

        private void OnValidate()
        {
            _actions = GetComponentsInChildren<SchemeAction>()
                .Where(a => a.transform.parent == transform)
                .ToArray();

            gameObject.name = $"> Foreach {_out?.name} in {_unitGroup?.name}";
        }
    }
}