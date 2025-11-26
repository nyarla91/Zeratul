using Source.Extentions.Input;
using Source.Extentions.Pause;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _actions;

        private InputBinding _selectMultipleBinding;
        private InputBinding _dragCameraBinding;
        
        public IBinding SelectMultipleBinding => _selectMultipleBinding;
        public IBinding DragCameraBinding => _dragCameraBinding;

        public float ZoomDelta => _actions.Player.ZoomDelta.ReadValue<float>();

        [Inject] public IPauseRead PauseRead { get; set; }

        private void Awake()
        {
            _actions =  new InputActions();
            _actions.Enable();
            _selectMultipleBinding = new InputBinding(_actions.Player.SelectMultiple, PauseRead);
            _dragCameraBinding = new InputBinding(_actions.Player.DragCamera, PauseRead);
        }

        private void OnDestroy()
        {
            _selectMultipleBinding.Dispose();
            _dragCameraBinding.Dispose();
        }
    }
}