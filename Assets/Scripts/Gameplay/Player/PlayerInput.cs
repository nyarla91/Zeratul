using System;
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
        
        public IBinding SelectMultipleBinding => _selectMultipleBinding;

        [Inject]
        public IPauseRead PauseRead { get; set; }

        private void Awake()
        {
            _actions =  new InputActions();
            _actions.Enable();
            _selectMultipleBinding = new InputBinding(_actions.Player.SelectMultiple, PauseRead);
        }

        private void OnDestroy()
        {
            _selectMultipleBinding.Dispose();
        }
    }
}