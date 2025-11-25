using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class NodeModifier : MonoBehaviour
    {
        [SerializeField] private bool _passable = true;
        [SerializeField] private int _extraTravelCost;

        public bool Passable => _passable;
        public int ExtraTravelCost => _extraTravelCost;
    }
}