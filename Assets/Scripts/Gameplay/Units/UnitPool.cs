using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitPool : IGetUnitByIdService
    {
        private readonly Dictionary<int, Unit> _units = new();

        public HashSet<Unit> Units => _units.Values.ToHashSet();
        
        public HashSet<Unit> PlayerUnits => Units.Where(u => u.Alliance.OwnedByPlayer).ToHashSet();
        
        public HashSet<Unit> EnemyUnits => Units.Where(u => u.Alliance.OwnedByEnemy).ToHashSet();
        
        public Unit GetUnitById(int id) => _units.GetValueOrDefault(id);

        public void AddUnit(Unit unit)
        {
            _units.Add(unit.Id, unit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit.Id);
        }
    }
    
    public interface IGetUnitByIdService
    {
        public Unit GetUnitById(int id);
    }
}