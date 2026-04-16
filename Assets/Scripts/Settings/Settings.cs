using System;
using System.IO;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "Settings")]
    public class Settings : ScriptableObject
    {
        [SerializeField] private AnimationCurve _soundCurve;
        [SerializeField] private string _savedFileName;
        [SerializeField] private SettingsConfig _config;

        public SettingsConfig Config => _config;

        private string SaveFilePath => Application.dataPath + "/" + _savedFileName + ".json";

        public event Action ConfigChanged;
        
        private void Awake()
        {
            if (TryLoad(out SettingsConfig loadedConfig))
                _config = loadedConfig;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
                Screen.fullScreenMode = FullScreenMode.Windowed;
            if (UnityEngine.Input.GetKeyDown(KeyCode.H))
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }

        private bool TryLoad(out SettingsConfig loadedConfig)
        {
            loadedConfig = new SettingsConfig();
            if (!File.Exists(SaveFilePath))
                return false;
            string json = File.ReadAllText(SaveFilePath);
            loadedConfig = JsonUtility.FromJson<SettingsConfig>(json);
            return true;
        }
    }
}