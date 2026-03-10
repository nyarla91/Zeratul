using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Vision Config", order = 0)]
    public class VisionConfig : ScriptableObject
    {
        [SerializeField] private int _unitVisionPoints;
        [SerializeField] private float _recalculationPeriod;
        [SerializeField] private LayerMask _visionBlockerMask;
        [SerializeField] private float _absoluteExtraSight;
        [SerializeField] private float _minSight;

        public int UnitVisionPoints => _unitVisionPoints;
        public float RecalculationPeriod => _recalculationPeriod;
        public LayerMask VisionBlockerMask => _visionBlockerMask;
        public float AbsoluteExtraSight => _absoluteExtraSight;
        public float MinSight => _minSight;
    }
}