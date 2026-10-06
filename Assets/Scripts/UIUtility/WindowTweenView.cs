using UnityEngine;

namespace UIUtility
{
    public class WindowTweenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void OnOpen()
        {
            transform.DOFadeIn(_canvasGroup);
        }

        public void OnClose()
        {
            transform.DOFadeOut(_canvasGroup);
        }
    }
}