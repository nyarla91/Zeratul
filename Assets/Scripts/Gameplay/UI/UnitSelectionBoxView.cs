using Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.UI
{
    public class UnitSelectionBoxView : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private UnitSelector _selector;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Update()
        {
            _canvasGroup.alpha = _selector.IsSelecting ? 1 : 0;
            if ( ! _selector.IsSelecting)
                return;

            Vector2 startingMousePosition = ScreenToCanvasPoint(_selector.SelectionStartPosition);
            Vector2 currentMousePosition = ScreenToCanvasPoint(Mouse.current.position.ReadValue());
            
            Vector2 boxOrigin = Vector2.Min(startingMousePosition, currentMousePosition);
            Vector2 boxSize = (currentMousePosition - startingMousePosition).Abs();
            
            _rectTransform.anchoredPosition = boxOrigin;
            _rectTransform.sizeDelta = boxSize;
        }

        private Vector2 ScreenToCanvasPoint(Vector2 screenPoint)
        {
            Rect canvasRect = _canvas.rect;
            return new Vector2
            (
                screenPoint.x.Remap(0, Screen.width, 0, canvasRect.width),
                screenPoint.y.Remap(0, Screen.height, 0, canvasRect.height)
            );
        }
    }
}