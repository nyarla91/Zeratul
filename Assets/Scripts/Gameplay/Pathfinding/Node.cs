using System.Linq;
using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class Node : INodeWorld
    {
        private const string GroundLayer = "GroundObstacle";
        private const string CommonLayer = "CommonObstacle";
        
        public Vector2 WorldPosition { get; }
        public Vector2Int MapCoordinates { get; }
        
        public int LastQuery { get; set; }
        public bool WasProcessedThisQuery { get; set; }
        public Node PreviousNode { get; set; }
        public int H { get; set; }
        public int G { get; set; }
        public int F => G + H;
        
        public bool IsPassableByGround { get; private set; }
        public bool IsPassableByAir { get; private set; }
        
        public Node(Vector2 worldPosition, Vector2Int mapCoordinates)
        {
            WorldPosition = worldPosition;
            MapCoordinates = mapCoordinates;
        }

        public void RecalculateObstacles()
        {
            Collider2D[] overlap = Physics2D.OverlapPointAll(WorldPosition);
            
            IsPassableByAir = ! IsAnyObstacle(overlap, CommonLayer);
            IsPassableByGround = IsPassableByAir && ! IsAnyObstacle(overlap, GroundLayer);
        }

        public bool IsPassable(bool isAgentAir) => isAgentAir ? IsPassableByAir : IsPassableByGround;

        private static bool IsAnyObstacle(Collider2D[] colliders, string layer) =>
            colliders.Any(col => col.gameObject.layer == LayerMask.NameToLayer(layer));
    }
}