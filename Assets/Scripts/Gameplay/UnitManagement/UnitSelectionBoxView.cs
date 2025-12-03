using Extentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.UnitManagement
{
    public class UnitSelectionBoxView : MonoBehaviour
    {
        [SerializeField]
        private UnitSelectior _model;
        
        [SerializeField]
        private RectTransform _rectTransform;
        
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Update()
        {
            _canvasGroup.alpha = _model.IsSelecting ? 1 : 0;
            if ( ! _model.IsSelecting)
                return;

            Vector2 startingMousePosition = _model.SelectionStartPosition;
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            
            Vector2 boxOrigin = Vector2.Min(startingMousePosition, currentMousePosition);
            Vector2 boxSize = (currentMousePosition - startingMousePosition).Abs();
            
            _rectTransform.anchoredPosition = boxOrigin;
            _rectTransform.sizeDelta = boxSize;
        }
    }
}