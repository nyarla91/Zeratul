using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Layers", order = 0)]
    public class LayersConfig : ScriptableObject
    {
        [SerializeField] private LayerMask _unitInteractionMask;

        public LayerMask UnitInteractionMask => _unitInteractionMask;
    }
}