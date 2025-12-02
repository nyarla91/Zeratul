using System;
using Gameplay.Pathfinding;
using Source.Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitMovement : UnitComponent
    {
        private const float NodeProximityDistance = 1;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CircleCollider2D _collider;
        
        private Vector2 _destination;
        private INodeWorld[] _path = Array.Empty<INodeWorld>();
        private int _nodesPassed;
        
        public bool HasPath => _path.Length > 0;

        [Inject] public NodeMap NodeMap { get; private set; }

        private void Awake()
        {
            _collider.radius = Composition.Type.Movement.Size;
        }

        public void Move(Vector2 destination)
        {
            NodeMap.TryFindPath(transform.position, destination, out _path, Composition.Type.Movement.Size);
            _nodesPassed = 0;
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _path = Array.Empty<INodeWorld>();
        }
        
        private void FixedUpdate()
        {
            if (!HasPath)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                return;
            }
            if (_nodesPassed == _path.Length)
            {
                Stop();
                return;
            }
            
            int nextNodeIndex = Mathf.Min(_nodesPassed, _path.Length - 1);
            if (_path[nextNodeIndex].WorldPosition.OrtogonalDistance(transform.position) < NodeProximityDistance)
                _nodesPassed = nextNodeIndex + 1;

            Vector2 delta = transform.DirectionTo2D(_path[nextNodeIndex].WorldPosition) * Composition.Type.Movement.MaxSpeed;
            _rigidbody.linearVelocity = delta;
        }

        private void OnDrawGizmos()
        {
            if (_path.Length <= 1)
                return;
            for (int i = _path.Length - 1; i >= 1; i--)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_path[i].WorldPosition, _path[i - 1].WorldPosition);
            }
        }
    }
}