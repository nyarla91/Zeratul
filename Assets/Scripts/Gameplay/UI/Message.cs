using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Sprite _errorSprite;
        [SerializeField] private Sprite _warningSprite;
        [SerializeField] private Sprite _successSprite;
        [SerializeField] private Sprite _infoSprite;

        private void Start()
        {
            _canvasGroup.alpha = 0;
        }

        public void Show(string text, MessageType type)
        {
            _image.sprite = type switch
            {
                MessageType.Error => _errorSprite,
                MessageType.Warning => _warningSprite,
                MessageType.Success => _successSprite,
                MessageType.Info => _infoSprite,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            _text.text = text;

            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, 0.2f).onComplete += () =>
            {
                _canvasGroup.DOFade(0, 2.5f);
            };
        }
    }
    
    public enum MessageType
    {
        Error,
        Warning,
        Success,
        Info
    }
}