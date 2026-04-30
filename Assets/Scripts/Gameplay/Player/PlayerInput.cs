using Extentions.Input;
using Extentions.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using InputBinding = Extentions.Input.InputBinding;

namespace Gameplay.Player
{
    public class PlayerInput
    {
        private readonly InputActions _actions;

        private InputBinding _selectMultiple;
        private InputBinding _queueOrder;
        private InputBinding _dragCamera;
        private InputBinding _focusNextUnitType;
        private InputBinding _toggleTacticalPause;
        private InputBinding _togglePause;
        private InputBinding _quickSave;
        private InputBinding _quickLoad;
        
        public IInputBindingReadonly SelectMultiple => _selectMultiple ??= new InputBinding(_actions.General.SelectMultiple, GamePause);
        public IInputBindingReadonly QueueOrder => _queueOrder ??= new InputBinding(_actions.General.QueueOrder, GamePause);
        public IInputBindingReadonly DragCamera => _dragCamera ??= new InputBinding(_actions.General.DragCamera, GamePause);
        public IInputBindingReadonly FocusNextUnitType => _focusNextUnitType ??= new InputBinding(_actions.General.FocusNextUnitType, GamePause);
        public IInputBindingReadonly TacticalPause => _toggleTacticalPause ??= new InputBinding(_actions.General.ToggleTacticalPause, GamePause);
        public IInputBindingReadonly TogglePause => _toggleTacticalPause ??= new InputBinding(_actions.General.TogglePause, GamePause);
        public IInputBindingReadonly QuickSave => _quickSave ??= new InputBinding(_actions.General.QuickSave, GamePause);
        public IInputBindingReadonly QuickLoad => _quickLoad ??= new InputBinding(_actions.General.QuickLoad, GamePause);

        public float ZoomDelta => _actions.General.ZoomDelta.ReadValue<float>();

        [Inject] public GamePause GamePause { get; set; }

        public PlayerInput()
        {
            _actions = new InputActions();
            _actions.Enable();
        }

        public InputAction GetOrderHotkeyAction(string alias) => _actions.FindAction(alias);

        public void Dispose()
        {
            _selectMultiple?.Dispose();
            _queueOrder?.Dispose();
            _dragCamera?.Dispose();
            _focusNextUnitType?.Dispose();
            _toggleTacticalPause?.Dispose();
            _togglePause?.Dispose();
            _quickSave?.Dispose();
            _quickLoad?.Dispose();
        }
    }
}