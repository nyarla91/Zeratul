using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerOwnership : MonoBehaviour
    {
        private List<UnitOwnership> _ownedUnits = new();

        public List<UnitOwnership> OwnedUnits => _ownedUnits.ToList();

        public void AddOwnedUnit(UnitOwnership unit)
        {
            if ( ! unit.OwnedByPlayer)
                return;
            _ownedUnits.Add(unit);
        }

        public void RemoveOwnedUnit(UnitOwnership unit)
        {
            if (unit.OwnedByPlayer)
                return;
            _ownedUnits.Remove(unit);
        }
        
        public bool IsUnitOwnedByPlayer(Unit unit) => _ownedUnits.Contains(unit.Ownership); 
    }
}