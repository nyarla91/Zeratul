using System;
using System.Collections.Generic;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionAddUnitGroupToUnitGroup : SchemeAction
    {
        [SerializeField] private SchemeValue<HashSet<Unit>> _source;
        [SerializeField] private VariableUnitGroup _target;
        
        public override void Act()
        {
            _target.AddGroup(_source?.Value);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Add {_source?.name} to {_target?.name}";
        }
    }
}