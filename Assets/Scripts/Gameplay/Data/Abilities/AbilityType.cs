using Gameplay.Data.Effects;
using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Abilities
{
    [CreateAssetMenu(menuName = "Gameplay Data/Ability", order = 0)]
    public class AbilityType : ScriptableObject
    {
        [Tooltip("Cooldown between uses (in fixed frames)")]
        [SerializeField] private int _cooldown;
        [Space]
        [SerializeField] private TargetRequirement _targetRequirement;
        [Tooltip("Cast distance to target (irrelevant when Target Requirement is set to None)")]
        [SerializeField] private float _maxDistance;
        [Header("Validators")]
        [Tooltip("Caster must pass these validators to cast this ability")]
        [SerializeField] private UnitValidatorGroup _casterValidators;
        [Tooltip("Target must pass these validators to be selected")]
        [SerializeField] private UnitValidatorGroup _targetValidators;
        [Header("Effects")]
        [Tooltip("Effects applied to caster itself")]
        [SerializeField] private EffectTargetingUnit[] _casterEffects;
        [Tooltip("Effects applied to caster's position")]
        [SerializeField] private EffectTargetingPoint[] _casterPointEffects;
        [Tooltip("Max delta angle towards target to cast this ability")]
        [SerializeField] private float _maxAngleToTarget = 360;
        [Tooltip("Effects applied to target unit")]
        [SerializeField] private EffectTargetingUnit[] _unitTargetEffects;
        [Tooltip("If checked Point Target Effects will also apply to target unit")]
        [SerializeField] private bool _applyPointEffectsToUnit = true;
        [Tooltip("Effects applied to target point")]
        [SerializeField] private EffectTargetingPoint[] _pointTargetEffects;

        public TargetRequirement TargetRequirement => _targetRequirement;
        public float MaxDistance => _maxDistance;
        public int Cooldown => _cooldown;
        public float MaxAngleToTarget => _maxAngleToTarget;

        public bool CanBeCast(Ability ability, OrderTarget target)
        {
            return _casterValidators.IsValid(ability.Caster, ability.Caster)
                   && ( ! target.Unit || _targetValidators.IsValid(ability.Caster, target.Unit))
                   && ability.IsReady;
        }
        
        public bool TryCast(Ability ability, OrderTarget target)
        {
            if ( ! ability.IsReady)
                return false;

            foreach (EffectTargetingUnit effect in _casterEffects)
            {
                effect.Apply(ability.Caster, ability.Caster);
            }
            foreach (EffectTargetingPoint effect in _casterPointEffects)
            {
                effect.Apply(ability.Caster, ability.Caster.transform.position);
            }

            if (target.Unit)
            {
                foreach (EffectTargetingUnit effect in _unitTargetEffects)
                {
                    effect.Apply(ability.Caster, target.Unit);
                }
                if (_applyPointEffectsToUnit)
                {
                    foreach (EffectTargetingPoint effect in _pointTargetEffects)
                    {
                        effect.Apply(ability.Caster, target.Unit.transform.position);
                    }
                }
            }
            else
            {
                foreach (EffectTargetingPoint effect in _pointTargetEffects)
                {
                    effect.Apply(ability.Caster, target.Point);
                }
            }
            
            ability.StartCooldown();
            return true;
        }

        private void OnValidate()
        {
            if (TargetRequirement != TargetRequirement.None)
                return;
            _maxDistance = 0;
            _maxAngleToTarget = 360;
        }
    }
}