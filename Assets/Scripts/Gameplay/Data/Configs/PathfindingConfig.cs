using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Pathfining Config", order = 0)]
    public class PathfindingConfig : ScriptableObject
    {
        [SerializeField] private Vector2 _mapOrigin;
        [SerializeField] private Vector2 _nodesWorldSpacing;
        [Space]
        [SerializeField] private int _ortogonalTravelCost;
        [SerializeField] private int _diagonalTravelCost;
        [SerializeField] private int _tooCloseToObstaclePenalty;
        [Space]
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private LayerMask _commonLayerMask;
        [SerializeField] private float _maxObstacleDistance;
        [SerializeField] private float _distanceCastStep;

        public Vector2 MapOrigin => _mapOrigin;
        public Vector2 NodesWorldSpacing => _nodesWorldSpacing;
        public int OrtogonalTravelCost => _ortogonalTravelCost;
        public int DiagonalTravelCost => _diagonalTravelCost;
        public int TooCloseToObstaclePenalty => _tooCloseToObstaclePenalty;
        public LayerMask GroundLayerMask => _groundLayerMask;
        public LayerMask CommonLayerMask => _commonLayerMask;
        public float MaxObstacleDistance => _maxObstacleDistance;
        public float DistanceCastStep => _distanceCastStep;
    }
}