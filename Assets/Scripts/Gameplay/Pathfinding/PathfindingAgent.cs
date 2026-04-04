using UnityEngine;

namespace Gameplay.Pathfinding
{
    public struct PathfindingAgent
    {
        public bool IsAir { get; }
        public float Radius { get; }

        public Vector2 BoundingBoxSize => new Vector2(Radius, Radius / 2);
        
        public PathfindingAgent(bool isAir, float radius)
        {
            IsAir = isAir;
            Radius = radius;
        }
    }
}