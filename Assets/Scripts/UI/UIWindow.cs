using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIWindow : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _startOpened;
        [SerializeField] private bool _closeOnEscape;
        [SerializeField] private UIWindow _previousWindow;

        public bool IsOpened => _canvasGroup.blocksRaycasts;
        
        private void Awake()
        {
            if (_closeOnEscape)
                Observable.EveryUpdate()
                    .Where(_ => IsOpened)
                    .Where(_ => Keyboard.current.escapeKey.wasPressedThisFrame)
                    .Delay(TimeSpan.FromSeconds(0.1f))
                    .Subscribe(_ => Close());
            
            if (_startOpened)
                Open();
            else
                Close();
        }

        public void Open()
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
        }
        
        public void Close()
        {
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _previousWindow?.Open();
        }
    }
}