using UnityEngine;

namespace Settings.UI
{
    public abstract class SettingsMenuItem : MonoBehaviour
    {
        [SerializeField] private string _key;

        public string Key => _key;

        public abstract void ApplyValue(int value);
        
        public abstract int GetValue();
        
        public abstract bool IsChanged();
    }
}