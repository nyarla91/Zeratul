using UnityEngine;

namespace Gameplay.Pathfinding
{
    public struct PathfindingAgent
    {
        public bool IsAir { get; }
        public float Radius { get; }

        public PathfindingAgent(bool isAir, float radius)
        {
            IsAir = isAir;
            Radius = radius;
        }
    }
}