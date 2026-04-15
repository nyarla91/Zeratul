using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace UIUtility
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<MenuWindow> _windows;
        [SerializeField] private bool _openedAtStart;
        [SerializeField] private MenuWindow _firstMenuWindow;

        public bool IsOpened { get; private set; }
        public MenuWindow CurrentWindow { get; private set; }

        public event Action Opened;
        public event Action Closed;

        protected virtual void Awake()
        {
            foreach (MenuWindow window in _windows)
            {
                window.Menu = this;
            }
        }

        protected virtual void Start()
        {
            Close(true);
            if (_openedAtStart)
                Open();
        }

        public void SwitchToWindow(MenuWindow menuWindow)
        {
            if ( ! _windows.Contains(menuWindow) || ! IsOpened)
                return;
            
            foreach (MenuWindow searchedWindow in _windows)
            {
                if (searchedWindow == menuWindow)
                {
                    searchedWindow.Open();
                    CurrentWindow = searchedWindow;
                    continue;
                }
                searchedWindow.Close();
            }
        }

        public void Open()
        {
            IsOpened = true;
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
            transform.DOAppear(_canvasGroup);
            if (_firstMenuWindow)
                SwitchToWindow(_firstMenuWindow);
            Opened?.Invoke();
        }

        public void Close(bool instant = false)
        {
            IsOpened = false;
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;

            if (instant)
                _canvasGroup.alpha = 0;
            else
                transform.DODisappear(_canvasGroup);

            foreach (MenuWindow searchedWindow in _windows)
            {
                searchedWindow.Close();
            }

            Closed?.Invoke();
        }
    }
}