using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace GameState
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }

        public async UniTask Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.DOComplete();
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1, _fadeDuration).AsyncWaitForCompletion();
        }
        
        public async UniTask Hide()
        {
            _canvasGroup.DOComplete();
            _canvasGroup.blocksRaycasts = false;
            await _canvasGroup.DOFade(0, _fadeDuration).AsyncWaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}