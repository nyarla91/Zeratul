using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Attacked", order = 0)]
    public class AttackedValidator : UnitValidator
    {
        [SerializeField] private float _maxTimeSinceTookDamage;

        public override bool IsValid(Unit actor, Unit target) =>
            Time.fixedTime - target.Life.LastDamageFrame < _maxTimeSinceTookDamage;
    }
}