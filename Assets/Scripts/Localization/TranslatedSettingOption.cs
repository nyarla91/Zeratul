using System;
using Settings.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace Localization
{
    public class TranslatedSettingOption : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private SettingsMenuItemList _list;

        [Inject] private Localizer Localizer { get; set; }
        
        private void Update()
        {
            _text.text = Localizer.Translate(_list.CurrentOption);
        }
    }
}