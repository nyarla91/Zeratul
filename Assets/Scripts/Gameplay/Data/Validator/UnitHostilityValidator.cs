using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Hostility", order = 0)]
    public class UnitHostilityValidator : UnitValidator
    {
        [SerializeField] private bool _mustBeHostile;

        public override bool IsValid(Unit actor, Unit target) =>
            _mustBeHostile ? target.Ownership.IsHostile(actor) : target.Ownership.IsFriendly(actor);
    }
}