using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Localization
{
    [CreateAssetMenu(menuName = "Localization/Language Table")]
    public class LanguageTable : ScriptableObject
    {
        [SerializeField] private LanguageEntry[] _entries;
        
        private Dictionary<string, LanguageEntry> _tableDictionary;

        public void Set(IEnumerable<LanguageEntry> entries)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                throw new InvalidOperationException("Set LocalizationTable only in edit mode");
            _entries = entries.ToArray();
            _tableDictionary = null;
#endif
        }

        public void GenerateDictionary()
        {
            _tableDictionary = _entries.ToDictionary(e => e.Key);
        }

        public string Translate(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;
            if (_tableDictionary == null)
                GenerateDictionary();
            return _tableDictionary.TryGetValue(key, out LanguageEntry entry) ? entry.Line : key;
        }
    }
}