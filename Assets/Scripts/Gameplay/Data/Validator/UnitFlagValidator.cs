using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Flag", order = 0)]
    public class UnitFlagValidator : UnitValidator
    {
        [SerializeField] private UnitFlag _flag;
        
        public override bool IsValid(Unit actor, Unit target) => target.GetFlag(_flag);
    }
}