using Settings.Localization;
using UnityEngine;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit Tag", order = 0)]
    public class UnitTag : ScriptableObject
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private bool _display;
        [SerializeField] private string _displayName;

        public bool Display => _display;
        public string DisplayName => _localizer.Translate(_displayName);
    }
}