using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Vision Config", order = 0)]
    public class VisionConfig : ScriptableObject
    {
        [SerializeField] private int _recalculationPeriod;
        [SerializeField] private LayerMask _visionBlockerMask;
        [SerializeField] private float _absoluteExtraSight;

        public int RecalculationPeriod => _recalculationPeriod;
        public LayerMask VisionBlockerMask => _visionBlockerMask;
        public float AbsoluteExtraSight => _absoluteExtraSight;
    }
}