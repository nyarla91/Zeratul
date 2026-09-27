using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Settings
{
    public class SettingsApplier
    {
        private readonly ISettingsReadService _settings;
        private readonly AudioMixer _audioMixer;

        [Inject]
        public SettingsApplier(ISettingsReadService settings, AudioMixer audioMixer)
        {
            _settings = settings;
            _audioMixer = audioMixer;
            _settings.ConfigChanged += UpdateValues;
            UpdateValues();
        }

        private async void UpdateValues()
        {
            await Task.Delay(100);
            Vector2Int resolution = GetResolutionForSettingsValue(_settings.Resolution);
            FullScreenMode fullScreenMode = _settings.Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.SetResolution(resolution.x, resolution.y, fullScreenMode);
            Debug.Log($"Screen set to {resolution} and {fullScreenMode}");
            
            UpdateSoundChannel(_settings.MasterVolume, "master");
            UpdateSoundChannel(_settings.MusicVolume, "music");
            UpdateSoundChannel(_settings.SfxVolume, "sfx");
        }

        private void UpdateSoundChannel(float percent, string mixerParameter)
        {
            percent = 1 - ((1 - percent) * (1 - percent));
            float db = Mathf.Lerp(-80, 0, percent);
            _audioMixer.SetFloat(mixerParameter, db);
            Debug.Log($"Audio mixer's {mixerParameter} is set to {db} dB");
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