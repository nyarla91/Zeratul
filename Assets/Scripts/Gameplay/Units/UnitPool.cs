using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitPool : MonoBehaviour
    {
        private readonly HashSet<Unit> _units = new();

        public HashSet<Unit> Units => _units.ToHashSet();
        
        public HashSet<Unit> PlayerUnits => _units.Where(u => u.Ownership.OwnedByPlayer).ToHashSet();
        
        public HashSet<Unit> EnemyUnits => _units.Where(u => ! u.Ownership.OwnedByPlayer).ToHashSet();
        
        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }
    }
}