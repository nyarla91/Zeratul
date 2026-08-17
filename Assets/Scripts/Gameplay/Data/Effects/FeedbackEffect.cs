using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Feedback", order = 0)]
    public class FeedbackEffect : EffectTargetingUnit
    {
        [SerializeField] private int _maxEnergyBurn;
        [SerializeField] private float _damageMultiplier;
        
        public override void Apply(Unit caster, Unit target)
        {
            int energyBurnt = Mathf.Min(target.Abilities.EnergyPoints, _maxEnergyBurn);
            target.Abilities.WasteEnergy(energyBurnt);
            int damage = Mathf.CeilToInt(_damageMultiplier * energyBurnt);
            target.Life?.TakeDamage(damage, caster);
        }
    }
}