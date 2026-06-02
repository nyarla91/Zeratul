using System;
using Settings;
using UnityEngine;
using Zenject;

namespace Localization
{
    [CreateAssetMenu(menuName = "Localization/Localizer")]
    public class Localizer : ScriptableObject
    {
        [SerializeField] private LanguageTable[] _tables;

        private LanguageTable CurrentTable => _tables[Settings?.Language ?? 0];

        [Inject] private ISettingsReadService Settings { get; set; }

        public string Translate(string key) => CurrentTable.Translate(key);

        public void GenerateDictionaries()
        {
            foreach (LanguageTable languageTable in _tables)
                languageTable.GenerateDictionary();
        }
    }
}