using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class NodeModifier : MonoBehaviour
    {
        [SerializeField] private bool _isPassable = true;
        [SerializeField] private int _extraTravelCost;

        public bool IsPassable => _isPassable;
        public int ExtraTravelCost => _extraTravelCost;
    }
}