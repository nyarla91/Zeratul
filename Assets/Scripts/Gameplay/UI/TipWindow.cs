using System;
using DG.Tweening;
using Gameplay.Data.Configs;
using Settings.Localization;
using TMPro;
using UIUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class TipWindow : MonoBehaviour
    {
        [SerializeField] private TextFormattingConfig _textFormattingConfig;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _pageNumber;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private ClickSound _clickSound;

        private TutorialEntry _entry;
        private int _currentPage;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
        }

        public void Show(TutorialEntry entry)
        {
            _entry = entry;
            _header.text = _localizer.Translate(entry.Header);
            
            transform.DOFadeIn(_canvasGroup);
            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = true;

            ShowPage(0);
            _clickSound.Play();
        }
        
        public void ShowNextPage() => ShowPage(_currentPage + 1);
        
        public void ShowPreviousPage() => ShowPage(_currentPage - 1);

        private void ShowPage(int page)
        {
            if (page < 0 || page >= _entry.Pages.Length)
                return;
            _currentPage = page;
            _description.text = _textFormattingConfig.Format(_localizer.Translate(_entry.Pages[page]));
            if (_entry.Pages.Length == 1)
            {
                _pageNumber.text = "";
                _nextButton.gameObject.SetActive(false);
                _previousButton.gameObject.SetActive(false);
                return;
            }
            _nextButton.gameObject.SetActive(true);
            _nextButton.interactable = page < _entry.Pages.Length - 1;
            _previousButton.gameObject.SetActive(true);
            _previousButton.interactable = page > 0;
            _pageNumber.text = $"{page + 1}/{_entry.Pages.Length}";
        }

        public void Hide()
        {
            transform.DOFadeOut(_canvasGroup);
            _canvasGroup.interactable = _canvasGroup.blocksRaycasts = false;
        }
    }
}