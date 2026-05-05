using System;
using Extentions.Pause;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.UI
{
    public class ClickArea : MonoBehaviour
    {
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _clickIndex;

        public event Action LeftClicked;
        public event Action RightClicked;
        
        [Inject] private GamePause GamePause { get; set; }
        
        private void Awake()
        {
            _eventTrigger.triggers[_clickIndex].callback.AddListener(_ => HandleClick());
        }

        private void HandleClick()
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