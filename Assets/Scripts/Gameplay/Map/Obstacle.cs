using UnityEngine;

namespace Gameplay.Map
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private bool _obstructsVision;

        public bool ObstructsVision => _obstructsVision;
    }
}