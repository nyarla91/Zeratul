using Gameplay.Data.Configs;
using Settings.Localization;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class TutorialElement : MonoBehaviour
    {
        [SerializeField] private TextFormattingConfig _textFormattingConfig;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _description;

        public void Set(TutorialEntry entry)
        {
            if (_header)
                _header.text = _localizer.Translate(entry.Header);
            if (_description)
                _description.text = _textFormattingConfig.Format(_localizer.Translate(entry.Description));
        }
    }
}