using System;
using Extentions;
using Gameplay.Schemes.Values;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public class ActionMoveCamera : SchemeAction
    {
        [SerializeField] private SchemeValue<Vector2> _position;
        
        public override void Act()
        {
            Camera mainCamera = Camera.main;
            if ( ! mainCamera)
                return;
            Vector3 position = _position.Value.WithZ(mainCamera.transform.position.z);
            mainCamera.transform.position = position;
        }

        private void OnValidate()
        {
            gameObject.name = $"> Move camera to {_position?.name}";
        }
    }
}