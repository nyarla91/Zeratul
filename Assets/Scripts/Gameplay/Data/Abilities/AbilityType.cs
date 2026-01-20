using Gameplay.Data.Effects;
using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using Gameplay.Units;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Abilities
{
    [CreateAssetMenu(menuName = "Gameplay Data/Ability", order = 0)]
    public class AbilityType : ScriptableObject
    {
        [Tooltip("Cooldown between uses (in fixed frames)")]
        [SerializeField] private int _cooldown;
        [SerializeField] private int _energyCost;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private TargetRequirement _targetRequirement;
        [Tooltip("Cast distance to target (irrelevant when Target Requirement is set to None)")]
        [SerializeField] private float _maxDistance;
        [HorizontalLine(2, EColor.White)]
        [Tooltip("Caster must pass these validators to cast this ability")]
        [SerializeField] private UnitValidatorGroup _casterValidators;
        [Tooltip("Target must pass these validators to be selected")]
        [SerializeField] private UnitValidatorGroup _targetValidators;
        [Tooltip("Effects applied to caster itself")]
        [SerializeField] private EffectTargetingUnit[] _casterEffects;
        [Tooltip("Effects applied to caster's position")]
        [SerializeField] private EffectTargetingPoint[] _casterPointEffects;
        [Tooltip("Max delta angle towards target to cast this ability")]
        [SerializeField] private float _maxAngleToTarget = 360;
        [Tooltip("Effects applied to target unit")]
        [SerializeField] private EffectTargetingUnit[] _unitTargetEffects;
        [Tooltip("Effects applied to target point")]
        [SerializeField] private EffectTargetingPoint[] _pointTargetEffects;

        public TargetRequirement TargetRequirement => _targetRequirement;
        public UnitValidatorGroup TargetValidators => _targetValidators;
        public float MaxDistance => _maxDistance;
        public int EnergyCost => _energyCost;
        public int Cooldown => _cooldown;
        public float MaxAngleToTarget => _maxAngleToTarget;

        public bool CanBeCast(Ability ability, OrderTarget target)
        {
            return _casterValidators.IsValid(ability.Caster, ability.Caster)
                   && (!target.Unit || _targetValidators.IsValid(ability.Caster, target.Unit))
                   && ability.Caster.Abilities.EnergyPoints >= EnergyCost
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
            }
            else
            {
                foreach (EffectTargetingPoint effect in _pointTargetEffects)
                {
                    effect.Apply(ability.Caster, target.Point);
                }
            }
            
            ability.StartCooldown();
            ability.Caster.Abilities.SpendEnergy(EnergyCost);
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