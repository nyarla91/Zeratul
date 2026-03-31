using System;
using UnityEngine;

namespace Localization
{
    [Serializable]
    public struct LanguageEntry
    {
        [SerializeField] private string _key;
        [SerializeField] private string _line;

        public string Key => _key;
        public string Line => _line;
        
        public LanguageEntry(string key, string line)
        {
            _key = key;
            _line = line;
        }
    }
}