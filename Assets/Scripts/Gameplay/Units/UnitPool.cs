using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree.Util;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitPool : IGetUnitByIdService
    {
        private readonly Dictionary<int, Unit> _units = new();

        public HashSet<Unit> Units => _units.Values.ToHashSet();
        
        public HashSet<Unit> PlayerUnits => Units.Where(u => u.Alliance.OwnedByPlayer).ToHashSet();
        
        public HashSet<Unit> EnemyUnits => Units.Where(u => u.Alliance.OwnedByEnemy).ToHashSet();

        public event Action<Unit> UnitAdded;
        public event Action<Unit> UnitRemoved;
        
        public Unit GetUnitById(int id) => _units.GetValueOrDefault(id);

        public void AddUnit(Unit unit)
        {
            if (_units.TryAdd(unit.Id, unit))
                UnitAdded?.Invoke(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            if (_units.Remove(unit.Id))
                UnitRemoved?.Invoke(unit);
        }
    }
    
    public interface IGetUnitByIdService
    {
        public Unit GetUnitById(int id);
    }
}