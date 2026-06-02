using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Settings
{
    [Serializable]
    public class SettingsConfig
    {
        [JsonProperty] private Dictionary<string, int> _values;

        public SettingsConfig(Dictionary<string, int> values)
        {
            _values = values;
        }
        
        public int GetValue(string key) => _values[key];

        public void ChangeValue(string key, int value) => _values[key] = value;
    }
}