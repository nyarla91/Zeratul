using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class PathfindingObstacle : MonoBehaviour
    {
        [SerializeField] private int _travelCost;

        public int TravelCost => _travelCost;
    }
}