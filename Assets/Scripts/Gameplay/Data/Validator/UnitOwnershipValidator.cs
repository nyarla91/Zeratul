using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Ownership", order = 0)]
    public class UnitOwnershipValidator : UnitValidator
    {
        [SerializeField] private bool _ownedByPlayer;
        
        public override bool IsValid(Unit unit) => unit.Ownership == _ownedByPlayer;
    }
}