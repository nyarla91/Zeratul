using _Core;
using UnityEngine;

namespace Gameplay.UI.Minimap
{
    public class MinimapCameraBoundView : MonoBehaviour
    {
        [SerializeField] private Minimap _minimap;
        [SerializeField] private Camera _mainCamera;

        private RectTransform _rectTransform; 

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Bounds cameraBounds = _mainCamera.GetCameraBounds();
            
            Vector2 worldPosition = cameraBounds.center;
            Vector2 worldSize = cameraBounds.size;

            _rectTransform.anchoredPosition = _minimap.WorldToMinimap(worldPosition);
            _rectTransform.sizeDelta = _minimap.WorldToMinimap(worldSize);
        }
    }
}