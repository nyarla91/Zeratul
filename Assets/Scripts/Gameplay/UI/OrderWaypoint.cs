using _Core;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class OrderWaypoint : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _line;
        [SerializeField] private float _lineWidth;

        private RectTransform RectTransform => _canvasGroup.transform as RectTransform;

        public bool IsHidden => _canvasGroup.alpha.Equals(0);
        
        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
        
        public void Draw(Sprite icon, Vector3 screenFrom, Vector3 screenTo)
        {
            _canvasGroup.alpha = 1;

            _icon.sprite = icon;
            RectTransform.anchoredPosition = screenTo;
            
            float lineLength = Vector2.Distance(screenFrom, screenTo);
            float lineAngle = ((Vector2) screenTo.DirectionTo(screenFrom)).ToDegrees();
            
            _line.rectTransform.sizeDelta =  new Vector2(lineLength, _lineWidth);
            _line.rectTransform.rotation = Quaternion.Euler(0, 0, lineAngle);
        }
    }
}