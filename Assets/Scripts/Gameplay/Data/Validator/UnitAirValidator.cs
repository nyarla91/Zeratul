using Gameplay.Units;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Air", order = 0)]
    public class UnitAirValidator : UnitValidator
    {
        [SerializeField] private bool _mustBeAir;
        
        public override bool IsValid(Unit actor, Unit target) => _mustBeAir == target.Type.IsAir;
    }
}