using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitPool : MonoBehaviour
    {
        private readonly HashSet<Unit> _units = new();

        public List<Unit> Units => _units.ToList();
        
        public List<Unit> PlayerUnits => _units.Where(u => u.Ownership.OwnedByPlayer).ToList();
        
        public List<Unit> EnemyUnits => _units.Where(u => ! u.Ownership.OwnedByPlayer).ToList();
        
        public void AddUnit(Unit unit)
        {
            if (_units.Contains(unit))
                return;
            _units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }
    }
}