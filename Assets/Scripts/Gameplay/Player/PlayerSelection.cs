using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerSelection
    {
        private List<Unit> _selectedUnits = new();
        private List<UnitType> _selectedUnitTypes = new();
        private int _focusedUnitTypeIndex;
        
        public UnitType FocusedUnitType => _selectedUnitTypes.Count > 0 ? _selectedUnitTypes[_focusedUnitTypeIndex] : null;
        
        public Unit[] SelectedUnits => _selectedUnits.ToArray();

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);
        
        public event Action SelectionUpdated;
        
        public PlayerSelection(PlayerInput input)
        {
            input.FocusNextUnitType.Performed += FocusNextUnitType;
        }
        
        public void AddUnitsToSelection(params Unit[] units)
        {
            foreach (Unit unit in units)
            {
                if (_selectedUnits.Contains(unit) || unit.Ownership.OwnedByEnemy)
                    continue;
                _selectedUnits.Add(unit);
                unit.Ownership.OwnerUpdated += ValidateSelectedUnits;
            }
            ValidateSelectedUnits();
        }

        public void RemoveUnitsFromSelection(params Unit[] units)
        {
            foreach (Unit unit in units)
            {
                _selectedUnits.Remove(unit);
                unit.Ownership.OwnerUpdated -= ValidateSelectedUnits;
            }
            ValidateSelectedUnits();
        }

        private void ClearSelection()
        {
            RemoveUnitsFromSelection(_selectedUnits.ToArray());
        }

        public void SelectUnits(params Unit[] units)
        {
            ClearSelection();
            AddUnitsToSelection(units);
        }

        public void ToggleUnitSelection(Unit  unit)
        {
            if (_selectedUnits.Contains(unit))
                RemoveUnitsFromSelection(unit);
            else
                AddUnitsToSelection(unit);
        }

        private void FocusNextUnitType()
        {
            _focusedUnitTypeIndex = (_focusedUnitTypeIndex + 1).RepeatIndex(_selectedUnitTypes.Count);
            Debug.Log(_focusedUnitTypeIndex);
            SelectionUpdated?.Invoke();
        }

        private void ValidateSelectedUnits(bool _) => ValidateSelectedUnits();

        private void ValidateSelectedUnits()
        {
            for (int i = _selectedUnits.Count - 1; i >= 0; i--)
            {
                Unit selectedUnit = _selectedUnits[i];

                if (!selectedUnit)
                {
                    _selectedUnits.RemoveAt(i);
                    continue;
                }
                if (selectedUnit.Ownership.OwnedByPlayer)
                    continue;
                
                RemoveUnitsFromSelection(selectedUnit);
            }

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