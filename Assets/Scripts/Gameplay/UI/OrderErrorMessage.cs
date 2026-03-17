using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class OrderErrorMessage : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;

        private void Start()
        {
            _canvasGroup.alpha = 0;
        }

        public void Show(string text)
        {
            _text.text = text;

            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, 0.2f).onComplete += () =>
            {
                _canvasGroup.DOFade(0, 2.5f);
            };
        }
    }
}