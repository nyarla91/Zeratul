using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Configs;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Player
{
    public class PlayerControlResources
    {
        private HashSet<Unit> _controlledUnits =  new();
        
        public int Reserve { get; private set; }
        public int Slots { get; private set; }
        
        public int OccupiedSlots => _controlledUnits.Sum(u => u.Type.ControlWorth);
        public int AvailableSlots => Slots - OccupiedSlots;
        public int ExtraReserve => Mathf.Max(Reserve - AvailableSlots, 0);
        
        public PlayerControlResources(PlayerControlConfig config)
        {
            Reserve = Mathf.Max(config.StartingReserve, 0);
            Slots = Mathf.Max(config.Slots, 0);

            Observable.EveryFixedUpdate()
                .Subscribe(_ => ValidateControlledUnits());

            Observable.EveryUpdate()
                .Where(_ => Keyboard.current.ctrlKey.isPressed)
                .Where(_ => Keyboard.current.rKey.wasPressedThisFrame)
                .Subscribe(_ => AddReserve(1));
        }

        public void AddReserve(int quantity)
        {
            if (quantity <= 0)
                return;
            Reserve += quantity;
        }

        public bool TrySpendReserve(int quantity)
        {
            if (quantity <= 0 || quantity > Reserve)
                return false;
            Reserve -= quantity;
            return true;
        }

        private void ValidateControlledUnits()
        {
            _controlledUnits = _controlledUnits.Where(u => u.Ownership.OwnedByPlayer && u.IsAlive).ToHashSet();
        }

        public bool CanFitUnit(Unit unit) => AvailableSlots >= unit.Type.ControlWorth;

        public bool TryAddUnit(Unit unit)
        {
            if ( ! CanFitUnit(unit))
                return false;
            return _controlledUnits.Add(unit);
        }
    }
}