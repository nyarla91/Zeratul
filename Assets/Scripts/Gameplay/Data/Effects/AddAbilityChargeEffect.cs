using System;
using Gameplay.Data.Abilities;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Add Ability Charges", order = 0)]
    public class AddAbilityChargeEffect : EffectTargetingUnit
    {
        [SerializeField] private AbilityType _abilityType;
        [SerializeField] private int _chargesAdded;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Abilities.GetAbility(_abilityType)?.AddCharges(_chargesAdded);
        }

        private void OnValidate()
        {
            _chargesAdded = Mathf.Max(0, _chargesAdded);
        }
    }
}