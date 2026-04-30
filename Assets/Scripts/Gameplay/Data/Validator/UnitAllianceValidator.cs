using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Alliance", order = 0)]
    public class UnitAllianceValidator : UnitValidator
    {
        [SerializeField] private ComparisonBehaviour _behaviour;

        public override bool IsValid(Unit actor, Unit target)
        {
            return _behaviour switch
            {
                ComparisonBehaviour.Friendly => actor.Alliance.IsFriendly(target),
                ComparisonBehaviour.Hostile => actor.Alliance.IsHostile(target),
                ComparisonBehaviour.SameOwner => actor.Alliance.CurrentOwner == target.Alliance.CurrentOwner,
                ComparisonBehaviour.DifferentOwner => actor.Alliance.CurrentOwner != target.Alliance.CurrentOwner,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private enum ComparisonBehaviour
        {
            Friendly,
            Hostile,
            SameOwner,
            DifferentOwner
        }
    }
}