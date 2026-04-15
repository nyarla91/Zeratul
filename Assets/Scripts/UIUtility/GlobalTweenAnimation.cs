using DG.Tweening;
using UnityEngine;

namespace UIUtility
{
    public static class GlobalTweenAnimation
    {
        private const float AppearTime = 0.3f;

        public static void DOAppear(this Transform target, CanvasGroup canvasGroup)
        {
            target.DOComplete();
            canvasGroup.DOComplete();
            canvasGroup.DOFade(1, AppearTime);
        }

        public static void DODisappear(this Transform target, CanvasGroup canvasGroup)
        {
            
            target.DOComplete();
            canvasGroup.DOComplete();
            canvasGroup.DOFade(0, AppearTime);
        }
    }
}