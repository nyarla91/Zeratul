using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Target Validator", order = 0)]
    public class TargetValidatorEffect : EffectTargetingUnit
    {
        [SerializeField] private UnitValidatorGroup _validators;
        [SerializeField] private EffectTargetingUnit[] _effects;
        
        public override void Apply(Unit caster, Unit target)
        {
            if (_validators.IsInvalid(caster, target))
                return;
            foreach (EffectTargetingUnit effect in _effects)
            {
                effect.Apply(caster, target);
            }
        }
    }
}