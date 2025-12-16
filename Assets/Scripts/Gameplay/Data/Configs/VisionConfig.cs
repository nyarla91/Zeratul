using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Fog Of War Config", order = 0)]
    public class VisionConfig : ScriptableObject
    {
        [SerializeField] private Color _hiddenColor;
        [SerializeField] private Color _scoutedColor;
        [SerializeField] private Color _revealedColor;
        [SerializeField] private LayerMask _revealMask;

        public LayerMask RevealMask => _revealMask;

        public Color GetColor(bool isRevealed, bool isScouted)
        {
            if (isRevealed)
                return _revealedColor;
            return isScouted ? _scoutedColor : _hiddenColor;
        }
    }
}