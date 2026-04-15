using Extentions;
using UnityEngine;

namespace UIUtility
{
    public class WindowTweenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void OnOpen()
        {
            transform.DOAppear(_canvasGroup);
        }

        public void OnClose()
        {
            transform.DODisappear(_canvasGroup);
        }
    }
}