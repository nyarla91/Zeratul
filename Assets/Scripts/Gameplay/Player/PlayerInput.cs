using Source.Extentions.Input;
using Source.Extentions.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using InputBinding = Source.Extentions.Input.InputBinding;

namespace Gameplay.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _actions;

        private InputBinding _selectMultipleBinding;
        private InputBinding _queueOrderBinding;
        private InputBinding _dragCameraBinding;
        
        public IBinding SelectMultipleBinding => _selectMultipleBinding;
        public IBinding QueueOrderBinding => _queueOrderBinding;
        public IBinding DragCameraBinding => _dragCameraBinding;

        public float ZoomDelta => _actions.General.ZoomDelta.ReadValue<float>();

        [Inject] public IPauseRead PauseRead { get; set; }

        private void Awake()
        {
            _actions =  new InputActions();
            _actions.Enable();
            _selectMultipleBinding = new InputBinding(_actions.General.SelectMultiple, PauseRead);
            _queueOrderBinding = new InputBinding(_actions.General.QueueOrder, PauseRead);
            _dragCameraBinding = new InputBinding(_actions.General.DragCamera, PauseRead);
        }

        private void OnDestroy()
        {
            _selectMultipleBinding.Dispose();
            _queueOrderBinding.Dispose();
            _dragCameraBinding.Dispose();
        }

        public InputAction GetOrderHotkeyAction(string alias) => _actions.FindAction(alias);
    }
}