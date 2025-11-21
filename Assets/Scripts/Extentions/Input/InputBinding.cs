using System;
using Source.Extentions.Pause;
using UnityEngine.InputSystem;

namespace Source.Extentions.Input
{
    public class InputBinding : IDisposable, IBinding
    {
        private bool _holdOnPerform;
        
        private readonly InputAction _action;

        private readonly IPauseRead _pause;
        
        public bool IsHeld { get; private set; }

        public event Action Pressed;
        public event Action Released;
        public event Action Performed;

        public InputBinding(InputAction action, IPauseRead pause, bool holdOnPerform = false)
        {
            _pause = pause;
            _action = action;
            _holdOnPerform = holdOnPerform;
            _action.started += OnPress;
            _action.performed += OnPerform;
            _action.canceled += OnRelease;
        }

        private void OnPress(InputAction.CallbackContext obj)
        {
            if (_pause.IsPaused)
                return;
            if  ( ! _holdOnPerform)
                IsHeld = true;
            Pressed?.Invoke();
        }

        private void OnPerform(InputAction.CallbackContext obj)
        {
            if (_pause.IsPaused)
                return;
            if  (_holdOnPerform)
                IsHeld = true;
            Performed?.Invoke();
        }

        private void OnRelease(InputAction.CallbackContext obj)
        {
            IsHeld = false;
            if (_pause.IsPaused)
                return;
            Released?.Invoke();
        }

        public void Dispose()
        {
            _action.started -= OnPress;
            _action.performed -= OnPerform;
            _action.canceled -= OnRelease;
        }
    }
}