using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Waste Energy", order = 0)]
    public class WasteEnergyEffect : EffectTargetingUnit
    {
        [SerializeField] private int _amount;
        
        public override void Apply(Unit caster, Unit target)
        {
            target.Abilities.WasteEnergy(_amount);
        }
    }
}