using System.Linq;
using Source.Extentions;
using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class Node
    {
        public Vector2 WorldPosition { get; }
        public Vector2Int MapCoordinates { get; }
        
        public int LastQuery { get; set; }
        public bool WasProcessedThisQuery { get; set; }
        public Node PreviousNode { get; set; }
        public int H { get; set; }
        public int G { get; set; }
        public int F => G + H;
        
        public int TravelCost { get; private set; }

        public Node(Vector2 worldPosition, Vector2Int mapCoordinates)
        {
            WorldPosition = worldPosition;
            MapCoordinates = mapCoordinates;
        }

        public void CalculateTravelCost()
        {
            Collider2D[] overlap = Physics2D.OverlapPointAll(WorldPosition);
            NodeModifier[] obstacles = overlap.Select(c => c.GetComponent<NodeModifier>()).ClearNull();
            if (obstacles.Length == 0)
            {
                TravelCost = 0;
                return;
            }
            TravelCost = obstacles.Max(ob => ob.ExtraTravelCost);
        }
    }
}