using UnityEngine;

namespace Settings.Localization
{
    [CreateAssetMenu(menuName = "Localization/Localizer")]
    public class Localizer : ScriptableObject
    {
        [SerializeField] private LanguageTable[] _tables;

        private LanguageTable CurrentTable => _tables[Language.Current];

        public string Translate(string key) => CurrentTable.Translate(key);

        public void GenerateDictionaries()
        {
            foreach (LanguageTable languageTable in _tables)
                languageTable.GenerateDictionary();
        }
    }
}