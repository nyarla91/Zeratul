
using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Extentions.Input;
using Gameplay.Units;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Player
{
    public class PlayerUnitRow : IDisposable
    {
        private readonly PlayerInput _input;
        private readonly PlayerSelection _selection;
        private readonly IDisposable _observer;

        private Unit[] _slots = new Unit[0];

        public Unit[] Slots => _slots;

        [Inject]
        public PlayerUnitRow(PlayerInput input, PlayerSelection selection, UnitPool unitPool)
        {
            _input = input;
            _selection = selection;
            for (int i = 0; i < input.SelectUnit.Length; i++)
            {
                InputBinding binding = input.SelectUnit[i];
                int unitIndex = i;
                binding.Pressed += () => SelectUnit(unitIndex);
            }

            Debug.Log(unitPool);

            _observer = Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateSlots(unitPool.PlayerUnits));
        }

        private void UpdateSlots(HashSet<Unit> units)
        {
            _slots = units
                .Where(u => u.IsInteractable)
                .OrderBy(u => -u.Type.FocusPriority * 10000 - u.Id)
                .ToArray();
        }

        private void SelectUnit(int index)
        {
            if (index < 0 || index >= _slots.Length)
                return;
            if (_input.SelectMultiple.IsHeld)
                _selection.ToggleUnitSelection(_slots[index]);
            else
                _selection.SelectUnits(_slots[index]);
        }

        public void Dispose()
        {
            _observer?.Dispose();
        }
    }
}