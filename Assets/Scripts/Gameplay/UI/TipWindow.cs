using DG.Tweening;
using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.UI
{
    public class TipWindow : TutorialElement
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Show(TutorialEntry entry)
        {
            Set(entry);
            
            _canvasGroup.DOComplete();
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.5f);
            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.DOComplete();
            _canvasGroup.DOFade(0, 0.5f);
            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
        }
    }
}