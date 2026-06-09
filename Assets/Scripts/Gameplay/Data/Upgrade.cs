using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Upgrade", order = 0)]
    public class Upgrade : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _displayName;
        [SerializeField] private string _displayDescription;

        public Sprite Icon => _icon;
        public string DisplayName => _displayName;
        public string DisplayDescription => _displayDescription;
    }
}