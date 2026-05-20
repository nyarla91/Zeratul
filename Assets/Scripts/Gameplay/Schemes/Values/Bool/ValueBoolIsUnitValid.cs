using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values.Bool
{
    public class ValueBoolIsUnitValid : SchemeValue<bool>
    {
        [SerializeField] private SchemeValue<Unit> _actor;
        [SerializeField] private SchemeValue<Unit> _target;
        [SerializeField] private UnitValidatorGroup _validator;
        [SerializeField] private bool _not;
        
        public override bool Value
        {
            get
            {
                if ( ! _target)
                    return false;
                if ( ! _actor)
                    _actor = _target;
                return _validator.IsValid(_actor.Value ?? _target.Value, _target.Value) != _not;
            }
        }

        private void OnValidate()
        {
            string not = _not ? "fail to" : "";
            gameObject.name = $"(Does {_target?.name} {not} meet {_validator} validators)";
        }
    }
}