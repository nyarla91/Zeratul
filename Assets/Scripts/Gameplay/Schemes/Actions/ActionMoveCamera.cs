using System;
using _Core;
using Gameplay.Map;
using Gameplay.Schemes.Values;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionMoveCamera : SchemeAction
    {
        [SerializeField] private SchemeValue<Vector2> _position;
        [SerializeField] private bool _moveImmediate;
        
        [Inject] private PlayerCamera _playerCamera;
        
        public override void Act()
        {
            _playerCamera.MoveTo(_position.Value, _moveImmediate);
        }

        private void OnValidate()
        {
            gameObject.name = $"> Move camera to {_position?.name}";
        }
    }
}