using Gameplay.Data.Statuses;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Status", order = 0)]
    public class UnitStatusValidator : UnitValidator
    {
        [SerializeField] private StatusType _status;
        
        public override bool IsValid(Unit actor, Unit target) => target.Statuses.HasStatus(_status);
    }
}