using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitComponentMono : MonoBehaviour
    {
        private Unit _unit;
        
        protected Unit Unit => _unit ??= GetComponent<Unit>();
        
        public UnitType UnitType => Unit.Type;
    }
}