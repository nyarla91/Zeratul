using System;
using _Core;
using Gameplay.Map;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.UI.Minimap
{
    public class MinimapCameraControl : MonoBehaviour
    {
        [SerializeField] private Minimap _minimap;
     
        [Inject] private PlayerCamera _playerCamera;
        
        public void MoveCamera(bool immediate)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 normalizedPosition = (mousePosition - (Vector2) _minimap.ScreenBounds.min) / _minimap.RectTransform.rect.size;
            Debug.Log(normalizedPosition);
            Vector2 worldPosition = _minimap.MapBounds.PointFromNormalized(normalizedPosition);
            
            _playerCamera.MoveTo(worldPosition, immediate);
        }
    }
}