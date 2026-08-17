using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Energy", order = 0)]
    public class UnitEnergyValidator : UnitPropertyValidator
    {
        protected override int GetUnitProperty(Unit unit) => unit.Abilities.EnergyPoints;
    }
}