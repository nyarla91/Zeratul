using System.Collections.Specialized;
using Extentions;
using Extentions.Pause;
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
        [SerializeField] private int _cooldown;
        [Space]
        [SerializeField] private TargetRequirement _targetRequirement;
        [SerializeField] private float _targetRadius;
        [Header("Validators")]
        [SerializeField] private UnitValidatorGroup _casterValidators;
        [SerializeField] private UnitValidatorGroup _targetValidators;
        [Header("Effects")]
        [SerializeField] private EffectTargetingUnit[] _casterEffects;
        [SerializeField] private EffectTargetingPoint[] _casterPointEffects;
        [SerializeField] private float _maxAngleToTarget = 360;
        [SerializeField] private EffectTargetingUnit[] _unitTargetEffects;
        [SerializeField] private bool _applyPointEffectsToUnit = true;
        [SerializeField] private EffectTargetingPoint[] _pointTargetEffect;

        public TargetRequirement TargetRequirement => _targetRequirement;
        public float TargetRadius => _targetRadius;
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
            }

            if (!target.Unit || _applyPointEffectsToUnit)
            {
                Vector2 point = target.Unit ? target.Unit.transform.position : target.Point;
                foreach (EffectTargetingPoint effect in _pointTargetEffect)
                {
                    effect.Apply(ability.Caster, point);
                }
            }
            
            ability.StartCooldown();
            return true;
        }
    }

    public class Ability
    {
        public AbilityType Type { get; }
        public Unit Caster { get; }
        public Timer CooldownTimer { get; }
        
        public bool IsReady => CooldownTimer.IsIdle;

        public Ability(AbilityType type, Unit caster, IPauseRead pauseRead)
        {
            Type = type;
            Caster = caster;
            CooldownTimer = new Timer(caster, Type.Cooldown, pauseRead);
        }

        public void StartCooldown()
        {
            CooldownTimer.Duration = Type.Cooldown;
            CooldownTimer.Restart();
        }
    }
}