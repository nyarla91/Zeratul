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

        private InputActions Actions => _actions ??= new InputActions();
        
        public IBinding SelectMultiple => _selectMultiple ??= new InputBinding(Actions.General.SelectMultiple, PauseRead);
        public IBinding QueueOrder => _queueOrder ??= new InputBinding(Actions.General.QueueOrder, PauseRead);
        public IBinding DragCamera => _dragCamera ??= new InputBinding(Actions.General.DragCamera, PauseRead);
        public IBinding FocusNextUnitType => _focusNextUnitType ??= new InputBinding(Actions.General.FocusNextUnitType, PauseRead);

        public float ZoomDelta => _actions.General.ZoomDelta.ReadValue<float>();

        [Inject] public IPauseRead PauseRead { get; set; }

        private void Awake()
        {
            Actions.Enable();
        }

        private void OnDestroy()
        {
            _selectMultiple.Dispose();
            _queueOrder.Dispose();
            _dragCamera.Dispose();
            _focusNextUnitType.Dispose();
        }

        public InputAction GetOrderHotkeyAction(string alias) => Actions.FindAction(alias);
    }
}