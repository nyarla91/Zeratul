using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Pathfinding;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitMovement : UnitComponent
    {
        private readonly NodeMap _nodeMap;
        private readonly TacticalPause _tacticalPause;
        private readonly UnitMovementConfig _config;
        private readonly Rigidbody2D _rigidbody;
        private readonly Collider2D _avoidanceCollider;

        private readonly Modifier _speedModifier = new Modifier();
        private Vector2 _destination;
        private INodeWorld[] _path = Array.Empty<INodeWorld>();
        private int _nodesPassed;
        private float _lastPathRecalculationTime;

        private Vector2 BoundingBoxSize => Isometry.Scale * UnitType.Size;
        public bool HasPath => _path.Length > 0;
        public bool Displaceable => ! HasPath && ! IsHoldingPosition && ! UnitType.IsImmobile;
        public Vector2 Velocity => _rigidbody.linearVelocity;
        public float LookAngle { get; private set; }
        public float TargetLookAngle { get; private set; }
        public bool IsHoldingPosition { get; private set; }
        public Modifier SpeedModifier => _speedModifier;
        public float Speed => UnitType.MaxSpeed * SpeedModifier.Value;

        public UnitMovement(Unit unit, TacticalPause tacticalPause, NodeMap nodeMap, UnitMovementConfig config,
            Rigidbody2D rigidbody, Collider2D collider, Collider2D avoidanceCollider) : base(unit)
        {
            _tacticalPause = tacticalPause;
            _nodeMap = nodeMap;
            _config = config;
            _rigidbody = rigidbody;
            _avoidanceCollider = avoidanceCollider;
            
            collider.transform.localScale = Vector3.one * UnitType.Size;
            Unit.gameObject.layer = UnitType.IsAir ? _config.AirLayer : _config.GroundLayer;
            collider.gameObject.layer = Unit.gameObject.layer;
            _avoidanceCollider.gameObject.layer = Unit.gameObject.layer;

            IObservable<long> observable = Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused);
            
            observable.Subscribe(_ => UpdatePhysics());
            observable.Subscribe(_ => UpdateRotation());
            observable.Subscribe(_ => MoveAlongPath());
        }

        public void Move(Vector2 destination)
        {
            if (UnitType.IsImmobile || HasPath && Time.time < _lastPathRecalculationTime + _config.MinPathRecalculationPeriod)
                return;
            _nodeMap.TryFindPath(Unit.Position, destination, out _path, UnitType.PathfindingAgent);
            //ReducePathToNecessary();
            if (_path.Length == 0 || HasReachedPoint(_path.Last().WorldPosition))
            {
                Stop();
                return; 
            }
            _lastPathRecalculationTime = Time.time;
            _nodesPassed = 0;
        }

        public void RotateTowards(Vector2 direction) => RotateTowards(direction.ToDegrees());

        public void RotateTowards(float angle) => TargetLookAngle = angle;

        public void Teleport(Vector2 position)
        {
            _rigidbody.MovePosition(position);
        }

        public void HoldPosition() => IsHoldingPosition = true;

        public void StopHoldingPosition() => IsHoldingPosition = false;

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _path = Array.Empty<INodeWorld>();
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

        private void UpdatePhysics()
        {
            _rigidbody.constraints = (IsHoldingPosition || UnitType.IsImmobile)
                ? RigidbodyConstraints2D.FreezeAll
                : RigidbodyConstraints2D.FreezeRotation;

            _rigidbody.mass = Displaceable ? 0.001f : 1;
        }

        private void UpdateRotation()
        {
            if (Unit.Stagger.IsStaggered)
                return;
            float maxDelta = UnitType.RotationSpeed * Time.fixedDeltaTime;
            LookAngle = Mathf.MoveTowardsAngle(LookAngle, TargetLookAngle, maxDelta);
        }

        private void MoveAlongPath()
        {
            if ( ! HasPath || Unit.Stagger.IsStaggered)
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
            if (HasReachedPoint(_path[nextNodeIndex].WorldPosition))
                _nodesPassed = nextNodeIndex + 1;

            Vector2 direction = Unit.Position.DirectionTo(_path[nextNodeIndex].WorldPosition);
            direction = AvoidObstaclesForDirection(direction);
            float speed = Speed * Mathf.Lerp(1, Isometry.VerticalScale, Mathf.Abs(direction.y));
            RotateTowards(direction / Isometry.Scale);
            _rigidbody.linearVelocity = direction * speed;
        }

        public bool HasReachedPoint(Vector2 point)
        {
            return point.OrthogonalDistance(Unit.Position) < UnitType.Size / 2 + _config.NodeProximityDistance;
        }

        private Vector2 AvoidObstaclesForDirection(Vector2 direction)
        {
            Collider2D[] overlap = new Collider2D[3];
            
            ContactFilter2D contactFilter = new()
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = UnitType.IsAir ? _config.AirLayerMask : _config.GroundLayerMask
            };
            
            int overlapTotal = _avoidanceCollider.Overlap(contactFilter, overlap);
            if (overlapTotal == 0)
                return direction;
            
            Unit[] obstacles = overlap.Select(col => col?.GetComponentInParent<Unit>()).NoNull();
            obstacles = obstacles.Where(unit => ! unit.Movement.Displaceable).ToArray();
            if (obstacles.Length == 0)
                return direction;
            float angle = direction.ToDegrees();
            Vector2[] oppositeDirections = obstacles
                .Select(obs => obs.Position.DirectionTo(Unit.Position))
                .ToArray();
            float oppositeAngle = oppositeDirections.Average().ToDegrees();
            float newAngle = Mathf.LerpAngle(angle, oppositeAngle, _config.AvoidanceStrength);
            return newAngle.DegreesToVector2().normalized;
        }
    }
}