using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerSelection : MonoBehaviour
    {

        private List<Unit> _selectedUnits = new();
        private List<UnitType> _selectedUnitTypes = new();
        private int _focusedUnitTypeIndex;
        
        public UnitType FocusedUnitType => _selectedUnitTypes.Count > 0 ? _selectedUnitTypes[_focusedUnitTypeIndex] : null;
        
        public Unit[] SelectedUnits => _selectedUnits.ToArray();

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);
        
        [Inject] private PlayerInput Input { get; set; }
        [Inject] private PlayerOwnership Ownership { get; set; }

        public event Action SelectionUpdated;

        private void Awake()
        {
            Debug.Log(Input);
            Input.FocusNextUnitType.Performed += FocusNextUnitType;
        }

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

        private void FocusNextUnitType()
        {
            _focusedUnitTypeIndex = (_focusedUnitTypeIndex + 1).RepeatIndex(_selectedUnitTypes.Count);
            Debug.Log(_focusedUnitTypeIndex);
            SelectionUpdated?.Invoke();
        }

        private void ValidateSelectedUnits()
        {
            _selectedUnits = _selectedUnits.Where(unit => unit.Ownership.OwnedByPlayer).ToList();
            _selectedUnits = _selectedUnits.ClearCopies().ToList();
            _selectedUnits = _selectedUnits.ClearNull().ToList();
            
            _selectedUnitTypes.Clear();
            foreach (Unit selectedUnit in _selectedUnits)
            {
                if (_selectedUnitTypes.Contains(selectedUnit.Type))
                    continue;
                _selectedUnitTypes.Add(selectedUnit.Type);
            }
            _selectedUnitTypes = _selectedUnitTypes.OrderBy(u => -u.FocusPriority).ToList();
            _focusedUnitTypeIndex = 0;
            
            SelectionUpdated?.Invoke();
        }
    }
}