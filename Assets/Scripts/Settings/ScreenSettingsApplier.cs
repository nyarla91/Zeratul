using System;
using UnityEngine;
using Zenject;

namespace Settings
{
    public class ScreenSettingsApplier
    {
        private readonly ISettingsReadService _settings;

        [Inject]
        public ScreenSettingsApplier(ISettingsReadService settings)
        {
            _settings = settings;
            _settings.ConfigChanged += UpdateScreen;
            UpdateScreen();
        }

        private void UpdateScreen()
        {
            Vector2Int resolution = GetResolutionForSettingsValue(_settings.Resolution);
            FullScreenMode fullScreenMode = _settings.Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.SetResolution(resolution.x, resolution.y, fullScreenMode);
            Debug.Log($"Screen set to {resolution} and {fullScreenMode}");
        }

        private Vector2Int GetResolutionForSettingsValue(int settingsValue)
        {
            return settingsValue switch
            {
                0 => new Vector2Int(1280, 720),
                1 => new Vector2Int(1366, 768),
                2 => new Vector2Int(1920, 1080),
                3 => new Vector2Int(2560, 1440),
                4 => new Vector2Int(3840, 2160),
                _ => throw new ArgumentOutOfRangeException(nameof(settingsValue), settingsValue, null)
            };
        }
    }
}