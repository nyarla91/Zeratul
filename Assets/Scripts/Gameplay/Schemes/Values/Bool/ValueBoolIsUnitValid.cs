using System;
using Extentions;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values
{
    public class ValueBoolIsUnitValid : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<Unit> _actor;
        [SerializeField] private SchemeValue<Unit> _target;
        [SerializeField] private UnitValidatorGroup _validator;
        [SerializeField] private bool _not;
        
        public override bool Value => _validator.IsValid(_actor.Value, _target.Value) != _not;

        private void OnValidate()
        {
            gameObject.name = $"Does ({_target?.name}) meet ({_validator}) validators";
        }
    }
}