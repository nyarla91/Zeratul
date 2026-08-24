using UnityEngine;

namespace Settings.Localization
{
    [CreateAssetMenu(menuName = "Localization/Localizer")]
    public class Localizer : ScriptableObject
    {
        [SerializeField] private LanguageTable[] _tables;

        private LanguageTable CurrentTable => _tables[Language.Current];

        public string Translate(string key)
        {
            if ( ! key.Contains('#'))
                return CurrentTable.Translate(key);
            string[] parts = key.Split('#');
            string text = CurrentTable.Translate(parts[0]);
            string[] replacements = parts[1].Split(',');
            foreach (string replacement in replacements)
            {
                if ( ! replacement.Contains('='))
                    continue;
                string[] replacementParts = replacement.Split('=');
                text = text.Replace(replacementParts[0], replacementParts[1]);
            }
            return text;
        }

        public void GenerateDictionaries()
        {
            foreach (LanguageTable languageTable in _tables)
                languageTable.GenerateDictionary();
        }
    }
}