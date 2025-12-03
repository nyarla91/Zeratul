using UnityEngine;

namespace Extentions
{
    public class RectTransformable : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>();
    }
}