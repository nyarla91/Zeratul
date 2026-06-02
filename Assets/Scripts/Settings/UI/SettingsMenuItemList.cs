using Extentions;
using TMPro;
using UnityEngine;

namespace Settings.UI
{
    public class SettingsMenuItemList : SettingsMenuItem
    {
        [SerializeField] private string[] _options;

        private int _lastValue;
        private int _currentValue;
        
        public string CurrentOption => _options[_currentValue];
        
        public override void ApplyValue(int value)
        {
            _lastValue = value;
            _currentValue = _lastValue;
        }

        public override int GetValue() => _currentValue;

        public override bool IsChanged() => _lastValue != _currentValue;

        public void SwitchRight() => _currentValue = (_currentValue + 1).RepeatIndex(_options.Length);
        
        public void SwitchLeft() => _currentValue = (_currentValue - 1).RepeatIndex(_options.Length);
    }
}