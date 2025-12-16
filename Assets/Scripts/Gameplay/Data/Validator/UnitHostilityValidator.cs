using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Hostility", order = 0)]
    public class UnitHostilityValidator : UnitValidator
    {
        [SerializeField] private bool _mustBeHostile;
        
        public override bool IsValid(Unit actor, Unit target)
        {
            bool isHostile = actor.Ownership.OwnedByPlayer != target.Ownership.OwnedByPlayer;
            return _mustBeHostile ?  isHostile : ! isHostile;
        }
    }
}