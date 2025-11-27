using System.Linq;
using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class Node : INodeWorld
    {
        private const string ObstacleLayer = "GroundObstacle";
        private const float MaxObstacleDistance = 3;
        private const float ObstacleDistanceStep = 0.25f;
        
        public Vector2 WorldPosition { get; }
        public Vector2Int MapCoordinates { get; }
        
        public int LastQuery { get; set; }
        public bool WasProcessedThisQuery { get; set; }
        public Node PreviousNode { get; set; }
        public int H { get; set; }
        public int G { get; set; }
        public int F => G + H;
        
        public bool Passable { get; private set; }
        public float DistanceToClosestObstacle { get; private set; }
        
        public Node(Vector2 worldPosition, Vector2Int mapCoordinates)
        {
            WorldPosition = worldPosition;
            MapCoordinates = mapCoordinates;
        }

        public void RecalculateObstacles()
        {
            Collider2D[] overlap = Physics2D.OverlapPointAll(WorldPosition);
            
            Passable = ! IsAnyObstacle(overlap);
            if ( ! Passable)
                return;

            DistanceToClosestObstacle = float.MaxValue;
            for (float radius = ObstacleDistanceStep; radius < MaxObstacleDistance; radius += ObstacleDistanceStep)
            {
                overlap = Physics2D.OverlapCircleAll(WorldPosition, radius);
                if ( ! IsAnyObstacle(overlap))
                    continue;
                DistanceToClosestObstacle = radius;
                break;
            }
        }

        private static bool IsAnyObstacle(Collider2D[] colliders) =>
            colliders.Any(col => col.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer));
    }
}