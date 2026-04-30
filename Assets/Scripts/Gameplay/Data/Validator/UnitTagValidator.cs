using System.Linq;
using Gameplay.Data.Units;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Tag", order = 0)]
    public class UnitTagValidator : UnitValidator
    {
        [SerializeField] private UnitTag _tag;
        
        public override bool IsValid(Unit actor, Unit target) => target.Type.Tags.Contains(_tag);
    }
}