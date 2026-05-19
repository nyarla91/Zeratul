using System;
using Extentions;
using Gameplay.Data.Effects;
using Gameplay.Schemes.Values;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionApplyEffectsTargetingUnit : SchemeAction
    {
        [SerializeField] private SchemeValue<Unit> _caster;
        [SerializeField] private SchemeValue<Unit> _target;
        [SerializeField] private EffectTargetingUnit[] _effects;
        
        public override void Act()
        {
            foreach (EffectTargetingUnit effect in _effects)
            {
                effect.Apply(_caster.Value, _target.Value);
            }
        }

        private void OnValidate()
        {
            gameObject.name = $"Apply ({_effects.Enumerate(", ", "", e => e.name)}) from ({_caster.name}) to ({_target.name})";
        }
    }
}