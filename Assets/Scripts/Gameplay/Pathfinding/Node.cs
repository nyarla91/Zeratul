using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class Node : INodeWorld
    {
        private const string GroundLayer = "GroundObstacle";
        private const string CommonLayer = "CommonObstacle";
        private const float MaxObstacleDistance = 4;
        private const float DistanceCastStep = 0.2f;
        
        public Vector2 WorldPosition { get; }
        public Vector2Int MapCoordinates { get; }
        
        public int LastQuery { get; set; }
        public bool WasProcessedThisQuery { get; set; }
        public Node PreviousNode { get; set; }
        public int H { get; set; }
        public int G { get; set; }
        public int F => G + H;
        
        public float GroundObstacleDistance { get; private set; }
        public float CommonObstacleDistance { get; private set; }

        public bool IsPassableByAir => CommonObstacleDistance > 0;
        public bool IsPassableByGround => GroundObstacleDistance > 0;
        
        public Node(Vector2 worldPosition, Vector2Int mapCoordinates)
        {
            WorldPosition = worldPosition;
            MapCoordinates = mapCoordinates;
        }

        public void RecalculateObstacles()
        {
            CommonObstacleDistance = ObstacleDistance(MaxObstacleDistance, LayerMask.GetMask(CommonLayer));
            GroundObstacleDistance = ObstacleDistance(CommonObstacleDistance, LayerMask.GetMask(GroundLayer));
            Debug.Log($"{CommonObstacleDistance} {GroundObstacleDistance}");
        }
        
        public float ObstacleDistanceFor(bool isAgentAir) => isAgentAir ? CommonObstacleDistance : GroundObstacleDistance;
        
        public bool IsPassable(bool isAgentAir) => isAgentAir ? IsPassableByAir : IsPassableByGround;
        
        private float ObstacleDistance(float maxRadius, LayerMask mask)
        {
            if (Physics2D.OverlapPoint(WorldPosition, mask))
                return 0;

            for (float i = maxRadius; i > 0; i -= DistanceCastStep)
            {
                if (Physics2D.OverlapCircle(WorldPosition, i, mask))
                    continue;
                return i;
            }
            return 0;
        }
    }
}