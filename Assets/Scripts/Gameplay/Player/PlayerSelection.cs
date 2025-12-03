using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerSelection : MonoBehaviour
    {

        private List<Unit> _selectedUnits = new();

        public Unit[] SelectedUnits => _selectedUnits.ToArray();

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);
        
        [Inject] private PlayerOwnership Ownership { get; set; }

        public event Action<Unit[]> SelectionUpdated;

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
            SelectionUpdated?.Invoke(SelectedUnits);
        }
    }
}