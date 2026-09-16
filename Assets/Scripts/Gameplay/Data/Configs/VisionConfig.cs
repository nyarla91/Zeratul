using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Vision Config", order = 0)]
    public class VisionConfig : ScriptableObject
    {
        [SerializeField] private int _visionPoints;
        [SerializeField] private int _visionCorrectionPoints;
        [SerializeField] private float _recalculationPeriod;
        [SerializeField] private LayerMask _visionBlockerMask;
        [SerializeField] private float _absoluteExtraSight;
        [SerializeField] private float _minSight;
        [SerializeField] private float _simulationRadius;
        [SerializeField] private float _fogPixelScale;
        [SerializeField] private Vector2Int _fogDimensions;

        public int VisionPoints => _visionPoints;
        public int VisionCorrectionPoints => _visionCorrectionPoints;
        public float RecalculationPeriod => _recalculationPeriod;
        public LayerMask VisionBlockerMask => _visionBlockerMask;
        public float AbsoluteExtraSight => _absoluteExtraSight;
        public float MinSight => _minSight;
        public float SimulationRadius => _simulationRadius;
        public float FogPixelScale => _fogPixelScale;
        public Vector2Int FogDimensions => _fogDimensions;
        
        public bool HasLineOfSight(Vector2 worldStart, Vector2 worldTarget, out RaycastHit2D hit)
        {
            Vector2 direction = worldTarget - worldStart;
            float distance = direction.magnitude;

            hit = Physics2D.Raycast(worldStart, direction, distance, _visionBlockerMask);
            return hit.collider == null;
        }
    }
}