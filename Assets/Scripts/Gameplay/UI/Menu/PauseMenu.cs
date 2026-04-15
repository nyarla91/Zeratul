using System;
using Extentions.Pause;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private UIUtility.Menu _menu;
        
        [Inject] private GamePause GamePause { get; set; }

        private void Awake()
        {
            Observable.EveryUpdate()
                .Where(_ => ! _menu.IsOpened)
                .Where(_ => Keyboard.current.escapeKey.wasReleasedThisFrame)
                .Delay(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => _menu.Open());

            _menu.Opened += () => GamePause.Pause(this);
            _menu.Closed += () => GamePause.Unpause(this);
        }
    }
}