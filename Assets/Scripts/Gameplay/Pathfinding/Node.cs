using System.Linq;
using Source.Extentions;
using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class Node : INodeWorld
    {
        private const string ObstacleLayer = "Obstacle";
        
        public Vector2 WorldPosition { get; }
        public Vector2Int MapCoordinates { get; }
        
        public int LastQuery { get; set; }
        public bool WasProcessedThisQuery { get; set; }
        public Node PreviousNode { get; set; }
        public int H { get; set; }
        public int G { get; set; }
        public int F => G + H;
        
        public bool Passable { get; private set; }

        public Node(Vector2 worldPosition, Vector2Int mapCoordinates)
        {
            WorldPosition = worldPosition;
            MapCoordinates = mapCoordinates;
        }

        public void CalculateTravelCost()
        {
            Collider2D[] overlap = Physics2D.OverlapPointAll(WorldPosition);
            Passable = overlap.All(col => col.gameObject.layer == LayerMask.NameToLayer(ObstacleLayer));
        }
    }
}