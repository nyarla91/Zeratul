using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Pathfinding;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitMovement : UnitComponent
    {
        [SerializeField] private UnitMovementConfig _config;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _avoidanceCollider;

        private Vector2 _destination;
        private INodeWorld[] _path = Array.Empty<INodeWorld>();
        private int _nodesPassed;
        private float _lastPathRecalculationTime;

        private Vector2 BoundingBoxSize => Isometry.Scale * UnitType.Movement.Size;
        public bool HasPath => _path.Length > 0;
        public bool Displacable => ! HasPath;
        public Vector2 Velocity => _rigidbody.linearVelocity;
        public float LookAngle { get; private set; }

        [Inject] public NodeMap NodeMap { get; private set; }


        private void Awake()
        {
            _collider.transform.localScale = Vector3.one * UnitType.Movement.Size;
        }

        public void Move(Vector2 destination)
        {
            if (HasPath && Time.time < _lastPathRecalculationTime + _config.MinPathRecalculationPeriod)
                return;
            NodeMap.TryFindPath(transform.position, destination, out _path, Composition.Type.Movement.Size / 2);
            //ReducePathToNecessary();
            _lastPathRecalculationTime = Time.time;
            _nodesPassed = 0;
        }

        public void RotateTowards(Vector2 direction, float deltaTime) => RotateTowards(direction.ToDegrees(), deltaTime);
        
        public void RotateTowards(float angle, float deltaTime)
        {
            float maxDelta = UnitType.Movement.RotationSpeed * Time.fixedDeltaTime;
            LookAngle = Mathf.MoveTowardsAngle(LookAngle, angle, maxDelta);
        }

        private void ReducePathToNecessary()
        {
            if (_path.Length <= 2)
                return;
            List<INodeWorld> necessaryPath = new(){_path[0]};
            ContactFilter2D contactFilter =  ContactFilter2D.noFilter;
            contactFilter.layerMask = LayerMask.GetMask("GroundObstacle");
            contactFilter.useLayerMask = true;
            for (int i = 1; i < _path.Length - 1; i++)
            {
                Vector2 lastNecessaryPoint = necessaryPath.Last().WorldPosition;
                float distance = Vector3.Distance(lastNecessaryPoint, _path[i].WorldPosition);
                Vector2 direction = (_path[i].WorldPosition - lastNecessaryPoint).normalized;
                RaycastHit2D[] hits = new RaycastHit2D[10];
                int hitsTotal = Physics2D.BoxCast(lastNecessaryPoint, BoundingBoxSize, 0, direction, contactFilter, hits, distance);
                if (hitsTotal > 0)
                {
                    necessaryPath.Add(_path[i - 1]);
                }
            }
            necessaryPath.Add(_path[^1]);
            _path = necessaryPath.ToArray();
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _path = Array.Empty<INodeWorld>();
        }

        private void FixedUpdate()
        {
            _rigidbody.mass = Displacable ? 0.001f : 1;
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
            if (_path[nextNodeIndex].WorldPosition.OrtogonalDistance(transform.position) < UnitType.Movement.Size / 2 + _config.NodeProximityDistance)
                _nodesPassed = nextNodeIndex + 1;

            Vector2 direction = transform.DirectionTo2D(_path[nextNodeIndex].WorldPosition);
            direction = AvoidObstaclesForDirection(direction);
            float speed = UnitType.Movement.MaxSpeed * Mathf.Lerp(1, Isometry.VerticalScale, Mathf.Abs(direction.y));
            RotateTowards(direction / Isometry.Scale, Time.fixedDeltaTime);
            _rigidbody.linearVelocity = direction * speed;
        }

        private Vector2 AvoidObstaclesForDirection(Vector2 direction)
        {
            Collider2D[] overlap = new Collider2D[3];
            ContactFilter2D contactFilter = new()
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = LayerMask.GetMask("Unit")
            };
            int overlapTotal = _avoidanceCollider.Overlap(contactFilter, overlap);
            if (overlapTotal == 0)
                return direction;
            Unit[] obstacles = overlap.Select(col => col?.GetComponentInParent<Unit>()).ClearNull();
            obstacles = obstacles.Where(unit => ! unit.Movement.Displacable).ToArray();
            if (obstacles.Length == 0)
                return direction;
            float angle = direction.ToDegrees();
            Vector2[] oppositeDirections = obstacles
                .Select(obs => obs.transform.DirectionTo2D(transform.position))
                .ToArray();
            float oppositeAngle = oppositeDirections.Average().ToDegrees();
            float newAngle = Mathf.LerpAngle(angle, oppositeAngle, _config.AvoidanceStrength);
            return newAngle.DegreesToVector2().normalized;
        }

        private void OnDrawGizmos()
        {
            if (_path.Length <= 1)
                return;
            for (int i = _path.Length - 1; i >= 1; i--)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_path[i].WorldPosition, _path[i - 1].WorldPosition);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_path[i].WorldPosition, 0.1f * Vector3.one);
            }
        }
    }
}