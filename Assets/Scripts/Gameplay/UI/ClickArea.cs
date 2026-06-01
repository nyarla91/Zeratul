using System;
using Extentions.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.UI
{
    public class ClickArea : MonoBehaviour
    {
        public event Action LeftClicked;
        public event Action RightClicked;
        
        [Inject] private GamePause GamePause { get; set; }
        
        public void HandleClick()
        {
            if (GamePause.IsPaused)
                return;
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                LeftClicked?.Invoke();
            if (Mouse.current.rightButton.wasReleasedThisFrame)
                RightClicked?.Invoke();
        }
    }
}