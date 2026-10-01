using System;
using _Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Sprite _errorSprite;
        [SerializeField] private Color _errorColor;
        [SerializeField] private AudioClip _errorClip;
        [SerializeField] private Sprite _warningSprite;
        [SerializeField] private Color _warningColor;
        [SerializeField] private AudioClip _warningClip;
        [SerializeField] private Sprite _successSprite;
        [SerializeField] private Color _successColor;
        [SerializeField] private AudioClip _successClip;
        [SerializeField] private Sprite _infoSprite;
        [SerializeField] private Color _infoColor;
        [SerializeField] private AudioClip _infoClip;
        [SerializeField] private float _pitchAmplitude;
        [SerializeField] private float _volumeScale;
        
        [Inject] private AudioSource AudioSource { get; set; }

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
            _image.color = type switch
            {
                MessageType.Error => _errorColor,
                MessageType.Warning => _warningColor,
                MessageType.Success => _successColor,
                MessageType.Info => _infoColor,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            AudioClip clip = type switch
            {
                MessageType.Error => _errorClip,
                MessageType.Warning => _warningClip,
                MessageType.Success => _successClip,
                MessageType.Info => _infoClip,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            _text.text = text;

            _canvasGroup.DOKill();
            _canvasGroup.DOFade(1, 0.2f).onComplete += () =>
            {
                _canvasGroup.DOFade(0, 2.5f);
            };
            AudioSource.PlayPitchedOneShot(clip, _pitchAmplitude, _volumeScale);
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