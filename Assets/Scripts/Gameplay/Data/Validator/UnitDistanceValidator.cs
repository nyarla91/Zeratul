using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Distance", order = 0)]
    public class UnitDistanceValidator : UnitValidator
    {
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        public override bool IsValid(Unit actor, Unit target)
        {
            float distance = Isometry.Distance(actor.Position, target);
            return distance >= _minDistance && distance <= _maxDistance;
        }
    }
}