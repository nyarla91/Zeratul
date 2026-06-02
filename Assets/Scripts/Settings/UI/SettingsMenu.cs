using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Settings.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private SettingsMenuItem[] _items;
        [SerializeField] private RectTransform _content;
        [SerializeField] private Button _discardButton;
        [SerializeField] private Button _applyButton;

        [Inject] private ISettingsReadService SettingsReadService { get; set; }
        [Inject] private ISettingsWriteService SettingsWriteService { get; set; }
        
        private void Start()
        {
            Discard();
        }
        
        public void Discard()
        {
            foreach (SettingsMenuItem item in _items)
            {
                int value = SettingsReadService.GetValue(item.Key);
                item.ApplyValue(value);
            }
        }

        public void ResetToDefault()
        {
            SettingsWriteService.ResetToDefault();
            Discard();
        }

        public void Apply()
        {
            foreach (SettingsMenuItem item in _items)
            {
                if ( ! item.IsChanged())
                    continue;
                SettingsWriteService.ChangeValue(item.Key, item.GetValue());
            }
            SettingsWriteService.Apply();
            Discard();
        }

        private void Update()
        {
            bool hasChanged = false;
            foreach (SettingsMenuItem item in _items)
            {
                if ( ! item.IsChanged())
                    continue;
                hasChanged = true;
                break;
            }
            _applyButton.interactable = hasChanged;
            _discardButton.interactable = hasChanged;
        }

        private void OnValidate()
        {
            _items = _content.GetComponentsInChildren<SettingsMenuItem>();
        }
    }
}