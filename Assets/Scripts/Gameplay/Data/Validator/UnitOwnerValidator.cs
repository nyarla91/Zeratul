using Extentions;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Owner", order = 0)]
    public class UnitOwnerValidator : UnitValidator
    {
        [SerializeField] private Owner _owner;
        
        public override bool IsValid(Unit actor, Unit target) => target.Alliance.CurrentOwner == _owner;
    }
}