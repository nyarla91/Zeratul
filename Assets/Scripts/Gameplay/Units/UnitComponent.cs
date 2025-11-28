using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitComponent : MonoBehaviour
    {
        private Unit _composition;
        
        protected Unit Composition => _composition ??= GetComponent<Unit>();
        
        public UnitType Type => Composition.Type;
    }
}