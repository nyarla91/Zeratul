using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UIUtility
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _canEscape = true;
        [SerializeField] private MenuWindow _previousMenuWindow;

        public bool IsOpened { get; private set; } = true;

        public Menu Menu { protected get; set; }

        public event Action Opened;
        public event Action Closed;

        private void Awake()
        {
            Close();
            if ( ! _canEscape)
                return;
            Observable.EveryUpdate()
                .Where(_ => IsOpened)
                .Where(_ => Keyboard.current.escapeKey.wasReleasedThisFrame)
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => OpenPreviousMenu());
        }

        public virtual void Open()
        {
            IsOpened = true;
            Opened?.Invoke();
            
            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
            transform.DOAppear(_canvasGroup);
        }

        public virtual void Close()
        {
            Closed?.Invoke();

            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
            transform.DODisappear(_canvasGroup);
            
            IsOpened = false;
        }

        public void OpenPreviousMenu()
        {
            if ( ! IsOpened)
                return;
            if (_previousMenuWindow)
                Menu.SwitchToWindow(_previousMenuWindow);
            else
                Menu?.Close();
        }
    }
}