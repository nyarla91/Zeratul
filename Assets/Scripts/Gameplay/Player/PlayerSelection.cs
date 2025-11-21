using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Source.Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerSelection : MonoBehaviour
    {

        private List<Unit> _selectedUnits = new();

        public List<Unit> SelectedUnits => _selectedUnits.ToList();

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);
        
        [Inject]
        public PlayerOwnership Ownership { get; set; }

        public void SelectUnits(Unit[] units)
        {
            _selectedUnits = units.ToList();
            ValidateSelectedUnits();
        }

        public void ToggleUnitSelection(Unit  unit)
        {
            if (IsUnitSelected(unit))
                RemoveUnitsFromSelection(new[] { unit });
            else
                AddUnitsToSelection(new[] { unit });
        }

        public void AddUnitsToSelection(Unit[] units)
        {
            _selectedUnits.AddRange(units);
            ValidateSelectedUnits();
        }

        public void RemoveUnitsFromSelection(Unit[] units)
        {
            foreach (Unit unit in units)
            {
                _selectedUnits.Remove(unit);
            }
            ValidateSelectedUnits();
        }

        private void ValidateSelectedUnits()
        {
            _selectedUnits = _selectedUnits.Where(unit => unit.Ownership.OwnedByPlayer).ToList();
            _selectedUnits = _selectedUnits.ClearCopies().ToList();
            _selectedUnits = _selectedUnits.ClearNull().ToList();
        }
    }
}