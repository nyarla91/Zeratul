using System;
using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(menuName = "Localization/Localizer")]
    public class Localizer : ScriptableObject
    {
        [SerializeField] private LanguageTable[] _tables;

        private LanguageTable CurrentTable => _tables[0];
        
        public string Translate(string key) => CurrentTable.Translate(key);
    }
}