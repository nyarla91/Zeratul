using UnityEngine;

namespace Gameplay.Pathfinding
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private bool _obstructsVision;

        public bool ObstructsVision => _obstructsVision;
    }
}