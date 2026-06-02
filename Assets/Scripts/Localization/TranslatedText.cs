using System;
using Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Localization
{
    public class TranslatedText : MonoBehaviour
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _tmp;
        [SerializeField] [TextArea(1, 10)] private string _text;

        [Inject] private ISettingsReadService Settings { get; set; }
        
        private void Awake()
        {
            Settings.ConfigChanged += Translate;
            Translate();
        }

        private void Translate()
        {
            _tmp.text = _localizer.Translate(_text);
        }

        private void OnDestroy()
        {
            Settings.ConfigChanged -= Translate;
        }

        private void OnValidate()
        {
            Translate();
        }
    }
}