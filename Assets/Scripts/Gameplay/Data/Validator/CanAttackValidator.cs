using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Can Attack", order = 0)]
    public class CanAttackValidator : UnitValidator
    {
        [SerializeField] private bool _mustAttack;
        
        public override bool IsValid(Unit actor, Unit target) => target.CanAttack == _mustAttack;
    }
}