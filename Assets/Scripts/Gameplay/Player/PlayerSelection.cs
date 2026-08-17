using System;
using System.Collections.Generic;
using System.Linq;
using _Core;
using Gameplay.Data.Units;
using Gameplay.Units;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerSelection
    {
        private List<Unit> _selectedUnits = new();
        private List<UnitType> _selectedUnitTypes = new();
        private int _focusedUnitTypeIndex;
        
        public UnitType FocusedUnitType => _selectedUnitTypes.Count > 0 ? _selectedUnitTypes[_focusedUnitTypeIndex] : null;
        
        public Unit[] SelectedUnits => _selectedUnits.ToArray();
        public Unit[] SelectedPlayerUnits => _selectedUnits.Where(u => u.Alliance.OwnedByPlayer).ToArray();
        
        public bool IsUncontrollableSelected => _selectedUnits.Count == 1 && ! _selectedUnits[0].Alliance.OwnedByPlayer;
        
        public event Action SelectionUpdated;
        public event Action<Unit> UnitSelectedTwice;

        [Inject]
        public PlayerSelection(PlayerInput input)
        {
            input.FocusNextUnitType.Performed += FocusNextUnitType;
        }

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);

        public void AddUnitsToSelection(params Unit[] units)
        {
            foreach (Unit unit in units)
            {
                if (_selectedUnits.Contains(unit))
                    continue;
                _selectedUnits.Add(unit);
                unit.Alliance.OwnerUpdated += ValidateSelectedUnits;
                unit.Killed += ValidateSelectedUnits;
            }
            ValidateSelectedUnits();
        }

        public void RemoveUnitsFromSelection(params Unit[] units)
        {
            foreach (Unit unit in units)
            {
                _selectedUnits.Remove(unit);
                unit.Alliance.OwnerUpdated -= ValidateSelectedUnits;
                unit.Killed -= ValidateSelectedUnits;
            }
            ValidateSelectedUnits();
        }

        private void ClearSelection()
        {
            RemoveUnitsFromSelection(_selectedUnits.ToArray());
        }

        public void SelectUnits(params Unit[] units)
        {
            if (_selectedUnits.Count == 1 && units.Length == 1 && _selectedUnits.First() == units.First())
            {
                UnitSelectedTwice?.Invoke(units.First());
            }
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
            SelectionUpdated?.Invoke();
        }

        private void ValidateSelectedUnits(Owner _) => ValidateSelectedUnits();

        private void ValidateSelectedUnits()
        {
            for (int i = _selectedUnits.Count - 1; i >= 0; i--)
            {
                Unit selectedUnit = _selectedUnits[i];

                if ( ! selectedUnit)
                {
                    _selectedUnits.RemoveAt(i);
                    continue;
                }
                if ( ! selectedUnit
                    || selectedUnit.IsDead 
                    || ! selectedUnit.CanBeTargetedByPlayer
                    || ( ! selectedUnit.Alliance.OwnedByPlayer && _selectedUnits.Count > 1))
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