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
        
        public void MoveCamera()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 normalizedPosition = (mousePosition - (Vector2) _minimap.ScreenBounds.min) / _minimap.RectTransform.rect.size;
            Debug.Log(normalizedPosition);
            Vector2 worldPosition = _minimap.MapBounds.PointFromNormalized(normalizedPosition);
            
            Camera mainCamera = Camera.main;
            Vector3 position = worldPosition.WithZ(mainCamera.transform.position.z);
            mainCamera.transform.position = position;
        }
    }
}