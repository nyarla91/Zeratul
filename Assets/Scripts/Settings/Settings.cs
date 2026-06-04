using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings")]
    public class Settings : ScriptableObject, ISettingsReadService, ISettingsWriteService
    {
        [SerializeField] private string _savedFileName;
        [SerializeField] private SerialazibleKeyValuePair<string, int>[] _defaultValues;

        private SettingsConfig _config;

        private string SaveFilePath => Application.dataPath + "/" + _savedFileName + ".json";
        
        public int Language => _config.GetValue("language");
        public bool Fullscreen => _config.GetValue("fullscreen") == 1;
        public int Resolution => _config.GetValue("resolution");
        public int CameraMoveSpeed => _config.GetValue("cameraMoveSpeed");
        public int CameraDragSpeed => _config.GetValue("cameraDragSpeed");

        public event Action ConfigChanged;

        private void Awake()
        {
            _config = LoadConfig();
            ConfigChanged?.Invoke();
        }

        public int GetValue(string key) => _config.GetValue(key);

        public void ChangeValue(string key, int value) => _config.ChangeValue(key, value);

        public void ResetToDefault()
        {
            _config = GetDefaultConfig();
            if (File.Exists(SaveFilePath))
                File.Delete(SaveFilePath);
            ConfigChanged?.Invoke();
        }

        public void Apply()
        {
            string json = JsonConvert.SerializeObject(_config);
            File.WriteAllText(SaveFilePath, json);
            ConfigChanged?.Invoke();
        }

        private SettingsConfig LoadConfig()
        {
            if ( ! File.Exists(SaveFilePath))
                return GetDefaultConfig();
            string json = File.ReadAllText(SaveFilePath);
            try
            {
                return JsonConvert.DeserializeObject<SettingsConfig>(json);
            }
            catch (JsonException e)
            {
                Debug.LogError(e.Message);
                return GetDefaultConfig();
            }
        }

        private SettingsConfig GetDefaultConfig()
        {
            Dictionary<string, int> values = _defaultValues.ToDictionary(p => p.key, p => p.value);
            return new SettingsConfig(values);
        }
    }

    public interface ISettingsReadService
    {
        public event Action ConfigChanged;
        public int GetValue(string key);
        public int Language { get; }
        public bool Fullscreen { get; }
        public int Resolution { get; }
        public int CameraMoveSpeed { get; }
        public int CameraDragSpeed { get; }
    }

    public interface ISettingsWriteService
    {
        public void ChangeValue(string key, int value);
        public void ResetToDefault();
        public void Apply();
    }
}