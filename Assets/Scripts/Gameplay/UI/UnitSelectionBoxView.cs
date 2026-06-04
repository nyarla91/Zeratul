using _Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.UI
{
    public class UnitSelectionBoxView : MonoBehaviour
    {
        [SerializeField] private RectTransform _hud;
        [SerializeField] private UnitSelector _selector;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Update()
        {
            _canvasGroup.alpha = _selector.IsSelecting ? 1 : 0;
            if ( ! _selector.IsSelecting)
                return;

            Vector2 startingMousePosition = _selector.SelectionStartPosition.ScreenToCanvasPoint(_hud);
            Vector2 currentMousePosition = Mouse.current.position.ReadValue().ScreenToCanvasPoint(_hud);
            
            Vector2 boxOrigin = Vector2.Min(startingMousePosition, currentMousePosition);
            Vector2 boxSize = (currentMousePosition - startingMousePosition).Abs();
            
            _rectTransform.anchoredPosition = boxOrigin;
            _rectTransform.sizeDelta = boxSize;
        }
    }
}