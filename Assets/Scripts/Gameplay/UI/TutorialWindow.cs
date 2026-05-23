using DG.Tweening;
using Gameplay.Data.Configs;
using Localization;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class TutorialWindow : MonoBehaviour
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TutorialRegistry _registry;
        [SerializeField] private TextFormattingConfig _config;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _description;
        
        public void Show(int index)
        {
            TutorialEntry entry = _registry.GetEntry(index);
            _label.text = _localizer.Translate(entry.Label);
            _description.text = _config.Format(_localizer.Translate(entry.Description));
            
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