using Extentions.Input;
using Extentions.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using InputBinding = Extentions.Input.InputBinding;

namespace Gameplay.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _actions;

        private InputBinding _selectMultiple;
        private InputBinding _queueOrder;
        private InputBinding _dragCamera;
        private InputBinding _focusNextUnitType;
        private InputBinding _tacticalPause;

        private InputActions Actions => _actions ??= new InputActions();
        
        public IInputBindingReadonly SelectMultiple => _selectMultiple ??= new InputBinding(Actions.General.SelectMultiple, GamePause);
        public IInputBindingReadonly QueueOrder => _queueOrder ??= new InputBinding(Actions.General.QueueOrder, GamePause);
        public IInputBindingReadonly DragCamera => _dragCamera ??= new InputBinding(Actions.General.DragCamera, GamePause);
        public IInputBindingReadonly FocusNextUnitType => _focusNextUnitType ??= new InputBinding(Actions.General.FocusNextUnitType, GamePause);
        public IInputBindingReadonly TacticalPause => _tacticalPause ??= new InputBinding(Actions.General.TacticalPause, GamePause);

        public float ZoomDelta => _actions.General.ZoomDelta.ReadValue<float>();

        [Inject] public GamePause GamePause { get; set; }

        private void Awake()
        {
            Actions.Enable();
        }

        private void OnDestroy()
        {
            _selectMultiple?.Dispose();
            _queueOrder?.Dispose();
            _dragCamera?.Dispose();
            _focusNextUnitType?.Dispose();
        }

        public InputAction GetOrderHotkeyAction(string alias) => Actions.FindAction(alias);
    }
}