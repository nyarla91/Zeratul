using System.Linq;
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
        [SerializeField] private int _windupTime;
        [SerializeField] private int _recoveryTime;
        [Tooltip("Cooldown between uses (in fixed frames)")]
        [SerializeField] private int _cooldown;
        [SerializeField] private AbilityCooldownGroup _cooldownGroup;
        [SerializeField] private int _energyCost;
        [SerializeField] private bool _ignoreLock;
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
        [Tooltip("Max delta angle towards target to cast this ability")]
        [SerializeField] private bool _mustLookAtTarget;
        [Tooltip("Effects applied to target unit")]
        [SerializeField] private EffectTargetingUnit[] _unitTargetEffects;
        [Tooltip("Effects applied to target point")]
        [SerializeField] private EffectTargetingPoint[] _pointTargetEffects;
        [SerializeField] private string _animationAction;

        public TargetRequirement TargetRequirement => _targetRequirement;
        public UnitValidatorGroup CasterValidators => _casterValidators;
        public UnitValidatorGroup TargetValidators => _targetValidators;
        public float MaxDistance => _maxDistance;
        public int EnergyCost => _energyCost;
        public bool IgnoreLock => _ignoreLock;
        public int WindupTime => _windupTime;
        public int RecoveryTime => _recoveryTime;
        public int Cooldown => _cooldownGroup?.Cooldown ?? _cooldown;
        public AbilityCooldownGroup CooldownGroup => _cooldownGroup;
        public bool MustLookAtTarget => _mustLookAtTarget;
        public EffectTargetingUnit[] CasterEffects => _casterEffects;
        public EffectTargetingUnit[] UnitTargetEffects => _unitTargetEffects;
        public EffectTargetingPoint[] PointTargetEffects => _pointTargetEffects;
        public string AnimationAction => _animationAction;

        public bool CanBeCast(Ability ability, OrderTarget target)
        {
            return _casterValidators.IsValid(ability.Caster, ability.Caster)
                   && ( ! target.Unit || _targetValidators.IsValid(ability.Caster, target.Unit))
                   && (ability.Caster.CanMove || IsTargetInRadius(ability.Caster, target))
                   && ability.Caster.Abilities.EnergyPoints >= EnergyCost
                   && (IgnoreLock || ability.Caster.Abilities.IsUnlocked) 
                   && ability.IsReady;
        }
        
        public bool IsTargetInRadius(Unit caster, OrderTarget target)
        {
            return TargetRequirement switch
            {
                TargetRequirement.None => true,
                TargetRequirement.Unit => Isometry.Distance(caster.Position, target.Unit) < MaxDistance,
                _ => Isometry.Distance(caster.Position, target.Point) < MaxDistance
            };
        }

        private void OnValidate()
        {
            _windupTime = Mathf.Max(0, _windupTime);
            _recoveryTime = Mathf.Max(0, _recoveryTime);
            _cooldown = _cooldownGroup ? 0 : Mathf.Max(0, _cooldown);
            _energyCost = Mathf.Max(0, _energyCost);
            if (TargetRequirement == TargetRequirement.None)
                _maxDistance = 0;
        }
    }
}