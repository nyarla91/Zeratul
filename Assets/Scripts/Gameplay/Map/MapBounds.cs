using Gameplay.Data.Configs;
using UnityEngine;

namespace Gameplay.Map
{
    public class MapBounds : MonoBehaviour
    {
        [SerializeField] private PathfindingConfig _config;
        [SerializeField] private NodeMap _nodeMap;
        [SerializeField] private BoxCollider2D _bounds;

        private void OnValidate()
        {
            _bounds.size = _config.NodesWorldSpacing * _nodeMap.MapSize;
            _bounds.offset = _bounds.size / 2 + _config.MapOrigin;
        }
    }
}