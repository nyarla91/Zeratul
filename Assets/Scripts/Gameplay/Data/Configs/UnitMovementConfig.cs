using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Unit Movement Config", order = 0)]
    public class UnitMovementConfig : ScriptableObject
    {
        [SerializeField] private float _nodeProximityDistance;
        [SerializeField] private float _minPathRecalculationPeriod;
        [SerializeField]  private float _avoidanceDistance;
        [SerializeField] [Range(0, 1)] private float _avoidanceStrength;
        [Space]
        [SerializeField] [Layer] private int _groundLayer;
        [SerializeField] [Layer] private int _airLayer;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _airMask;
        [SerializeField] private LayerMask _groundObstacleMask;
        [SerializeField] private LayerMask _commonObstacleMask;

        public float NodeProximityDistance => _nodeProximityDistance;
        public float MinPathRecalculationPeriod => _minPathRecalculationPeriod;
        public float AvoidanceDistance => _avoidanceDistance;
        public float AvoidanceStrength => _avoidanceStrength;
        public int GroundLayer => _groundLayer;
        public int AirLayer => _airLayer;
        public LayerMask GroundMask => _groundMask;
        public LayerMask AirMask => _airMask;
        public LayerMask GroundObstacleMask => _groundObstacleMask;
        public LayerMask CommonObstacleMask => _commonObstacleMask;
    }
}