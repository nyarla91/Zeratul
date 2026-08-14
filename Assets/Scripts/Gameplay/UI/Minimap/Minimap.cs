using _Core;
using UnityEngine;

namespace Gameplay.UI.Minimap
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private Camera _minimapCamera;

        public RectTransform RectTransform { get; private set; }
        public Bounds MapBounds { get; private set; }
        public Bounds ScreenBounds { get; private set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            MapBounds = _minimapCamera.GetCameraBounds();
            
            Vector3[] corners = new Vector3[4];
            RectTransform.GetWorldCorners(corners);
            ScreenBounds = new Bounds(Vector3.Lerp(corners[0], corners[2], 0.5f), corners[2] - corners[0]);
            Debug.Log($"{ScreenBounds.min} {ScreenBounds.max} {ScreenBounds.center} {ScreenBounds.size}");
        }

        public Vector2 WorldToMinimap(Vector2 worldPosition)
        {
            Vector2 t = (worldPosition - (Vector2) MapBounds.min) / MapBounds.size;
            return t * RectTransform.rect.size;
        }

        public Vector2 MinimapToWorld(Vector2 minimapPosition)
        {
            Vector2 t = minimapPosition / RectTransform.rect.size;
            return t * MapBounds.size + (Vector2) MapBounds.min;
        }
    }
}