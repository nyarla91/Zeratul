using TMPro;
using UnityEngine;

namespace Localization
{
    public class TranslatedText : MonoBehaviour
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _tmp;
        [SerializeField] [TextArea(1, 10)] private string _text;

        private void Awake()
        {
            Translate();
        }

        private void Translate()
        {
            _tmp.text = _localizer.Translate(_text);
        }

        private void OnValidate()
        {
            Translate();
        }
    }
}