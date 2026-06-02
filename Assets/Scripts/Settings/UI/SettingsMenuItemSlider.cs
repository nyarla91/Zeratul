using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.UI
{
    public class SettingsMenuItemSlider : SettingsMenuItem
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        private int _lastValue;

        public override void ApplyValue(int value)
        {
            _lastValue = value;
            _slider.value = _lastValue;
        }

        public override int GetValue() => Mathf.RoundToInt(_slider.value);

        public override bool IsChanged() => ! Mathf.Approximately(_slider.value, _lastValue);

        private void Update()
        {
            _number.text = Mathf.RoundToInt(_slider.value).ToString();
        }

        private void OnValidate()
        {
            _min = Mathf.Max(_min, 0);
            _slider.minValue = _min;
            _max = Mathf.Max(_min + 1, _max);
            _slider.maxValue = _max;
        }
    }
} 