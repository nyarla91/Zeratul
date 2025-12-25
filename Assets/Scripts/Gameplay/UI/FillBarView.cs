using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class FillBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectMask2D _mask;
        [SerializeField] private bool _hideOnMax;
        
        public void UpdateBar(float percent)
        {
            if (_hideOnMax && percent.Equals(1))
            {
                Hide();
                return;
            }
            _canvasGroup.alpha = 1;
            float maxPadding = _rectTransform.rect.width;
            float padding = (1 - percent) * maxPadding;
            _mask.padding = new Vector4(0, 0, padding, 0);
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }
    }
}