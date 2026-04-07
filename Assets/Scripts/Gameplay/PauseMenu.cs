using System;
using Extentions.Pause;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private UIWindow _window;
        
        [Inject] private GamePause GamePause { get; set; }
        
        private void Awake()
        {
            Observable.EveryUpdate()
                .Where(_ => ! _window.IsOpened)
                .Where(_ => Keyboard.current.escapeKey.wasPressedThisFrame)
                .Delay(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => _window.Open());

            _window.ObserveEveryValueChanged(w => w.IsOpened)
                .Subscribe(UpdatePause);
        }

        private void UpdatePause(bool pause)
        {
            if (pause)
                GamePause.Pause(this);
            else
                GamePause.Unpause(this);
        }
    }
}