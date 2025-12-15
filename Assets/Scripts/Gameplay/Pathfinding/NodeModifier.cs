using System;
using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class NodeModifier : MonoBehaviour
    {
        [SerializeField] private bool _isGroundObstacle = true;
        [SerializeField] private bool _isCommonObstalcle;
        [SerializeField] private int _extraTravelCost;

        public bool IsGroundObstacle => _isGroundObstacle;
        public bool IsCommonObstalcle => _isCommonObstalcle;
        public int ExtraTravelCost => _extraTravelCost;

        private void OnValidate()
        {
            if (_isCommonObstalcle)
                _isGroundObstacle = true;
        }
    }
}