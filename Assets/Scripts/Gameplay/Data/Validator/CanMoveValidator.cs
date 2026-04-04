using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Can Move", order = 0)]
    public class CanMoveValidator : UnitValidator
    {
        [SerializeField] private bool _mustMove;
        
        public override bool IsValid(Unit actor, Unit target) => target.CanMove == _mustMove;
    }
}