using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Unit Movement Config", order = 0)]
    public class UnitMovementConfig : ScriptableObject
    {
        [SerializeField] private float _nodeProximityDistance;
        [SerializeField] private float _minPathRecalculationPeriod;
        [SerializeField] [Range(0, 1)] private float _avoidanceAdditionalRadius;
        [SerializeField] [Range(0, 1)] private float _avoidanceStrength;
        [Space]
        [SerializeField] [Layer] private int _groundLayer;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] [Layer] private int _airLayer;
        [SerializeField] private LayerMask _airLayerMask;

        public float NodeProximityDistance => _nodeProximityDistance;
        public float MinPathRecalculationPeriod => _minPathRecalculationPeriod;
        public float AvoidanceAdditionalRadius => _avoidanceAdditionalRadius;
        public float AvoidanceStrength => _avoidanceStrength;
        public int GroundLayer => _groundLayer;
        public LayerMask GroundLayerMask => _groundLayerMask;
        public int AirLayer => _airLayer;
        public LayerMask AirLayerMask => _airLayerMask;
    }
}