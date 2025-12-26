using System;
using System.Linq;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Text Formatting", order = 0)]
    public class TextFormattingConfig : ScriptableObject
    {
        [SerializeField] private FormatPair[] _pairs;

        public string Format(string text)
        {
            return _pairs.Aggregate(text, (current, pair) => pair.Format(current));
        }
        
        [Serializable]
        private struct FormatPair
        {
            [SerializeField] private string _key;
            [SerializeField] private string _value;
            
            public string Format(string text) => text.Replace(_key, _value);
        }
    }

}