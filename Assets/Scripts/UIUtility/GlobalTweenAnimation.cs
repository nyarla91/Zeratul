using DG.Tweening;
using UnityEngine;

namespace UIUtility
{
    public static class GlobalTweenAnimation
    {
        private static readonly Vector3 FadeOutScale = new(4, 1, 1);
        private const float FadeTime = 0.3f;

        public static void DOFadeIn(this Transform target, CanvasGroup canvasGroup)
        {
            target.DOComplete();
            target.localPosition = new Vector3(0, -200, 0);
            target.DOLocalMove(Vector3.zero, FadeTime)
                .SetEase(Ease.OutSine);
            canvasGroup.DOComplete();
            canvasGroup.DOFade(1, FadeTime);
        }

        public static void DOFadeOut(this Transform target, CanvasGroup canvasGroup)
        {
            
            target.DOComplete();
            target.localPosition = Vector3.zero;
            target.DOLocalMove(new Vector3(0, 200, 0), FadeTime)
                .SetEase(Ease.OutSine);
            canvasGroup.DOComplete();
            canvasGroup.DOFade(0, FadeTime);
        }
    }
}