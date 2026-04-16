using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class SettingsConfig
    {
        [SerializeField] private SettingsEntry[] _settings;
    }

    public struct SettingsEntry
    {
        [SerializeField] private string _key;
        [SerializeField] private int _value;
        
        public string Key => _key;
        public int Value
        {
            get => _value;
            set => _value = value;
        }
    }
}