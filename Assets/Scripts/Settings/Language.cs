using Settings.Localization;
using UnityEngine;
using Zenject;

namespace Settings
{
    public class Language
    {
        public static int Current { get; private set; }
        
        private readonly ISettingsReadService _settings;
        
        [Inject]
        public Language(ISettingsReadService settings)
        {
            _settings = settings;
            settings.ConfigChanged += UpdateCurrent;
            UpdateCurrent();
        }

        private void UpdateCurrent()
        {
            Current = _settings.Language;
        }
    }
}