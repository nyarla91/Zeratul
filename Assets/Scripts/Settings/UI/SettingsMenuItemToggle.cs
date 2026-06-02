using UnityEngine;
using UnityEngine.UI;

namespace Settings.UI
{
    public class SettingsMenuItemToggle : SettingsMenuItem
    {
        [SerializeField] private Toggle _toggle;

        private bool _lastValue;
        
        public override void ApplyValue(int value)
        {
            _lastValue = value == 1;
            _toggle.isOn = _lastValue;
        }

        public override int GetValue() => _toggle.isOn ? 1 : 0;

        public override bool IsChanged() => _lastValue != _toggle.isOn;
    }
}