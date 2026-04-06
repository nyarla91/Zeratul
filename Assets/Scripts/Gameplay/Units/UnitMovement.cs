using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Map;
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
        private List<Vector2> _path = new();
        private float _lastPathRecalculationTime;

        private Vector2 BoundingBoxSize => Isometry.Scale * UnitType.Size;
        public bool HasPath => _path.Count > 0;
        public bool Displaceable => ! HasPath && ! IsHoldingPosition && ! UnitType.IsImmobile;
        public Vector2 Velocity => _rigidbody.linearVelocity;
        public bool IsHoldingPosition { get; private set; }
        public Modifier SpeedModifier => _speedModifier;
        public float Speed => UnitType.MaxSpeed * SpeedModifier.Value;

        public UnitMovement(Unit unit, TacticalPause tacticalPause, NodeMap nodeMap, UnitMovementConfig config,
            Rigidbody2D rigidbody, Collider2D avoidanceCollider) : base(unit)
        {
            _tacticalPause = tacticalPause;
            _nodeMap = nodeMap;
            _config = config;
            _rigidbody = rigidbody;
            _avoidanceCollider = avoidanceCollider;
            _avoidanceCollider.gameObject.layer = Unit.gameObject.layer;

            IObservable<long> observable = Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused);
            
            observable.Subscribe(_ => MoveAlongPath());
            observable.Subscribe(_ => UpdatePhysics());
        }

        public void Move(Vector2 destination)
        {
            if (UnitType.IsImmobile || HasPath && Time.time < _lastPathRecalculationTime + _config.MinPathRecalculationPeriod)
                return;
            _nodeMap.TryFindPath(Unit.Position, destination, out _path, UnitType.PathfindingAgent);
            if (_path.Count == 0 || HasReachedPoint(_path.Last()))
            {
                Stop();
                return; 
            }
            _lastPathRecalculationTime = Time.time;
        }

        public void Teleport(Vector2 position)
        {
            _rigidbody.MovePosition(position);
        }

        public void HoldPosition() => IsHoldingPosition = true;

        public void StopHoldingPosition() => IsHoldingPosition = false;

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _path = new List<Vector2>();
        }

        private void UpdatePhysics()
        {
            _rigidbody.constraints = (IsHoldingPosition || UnitType.IsImmobile)
                ? RigidbodyConstraints2D.FreezeAll
                : RigidbodyConstraints2D.FreezeRotation;

            _rigidbody.mass = Displaceable ? 0.001f : 1;
        }

        private void MoveAlongPath()
        {
            if ( ! HasPath || Unit.Stagger.IsStaggered)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                return;
            }
            
            if (HasReachedPoint(_path.First()))
                _path.RemoveAt(0);
            
            if (_path.Count == 0)
            {
                Stop();
                return;
            }

            Vector2 direction = Unit.Position.DirectionTo(_path.First());
            direction = AvoidObstaclesForDirection(direction);
            float speed = Speed * Mathf.Lerp(1, Isometry.VerticalScale, Mathf.Abs(direction.y));
            Unit.Direction.RotateTowards(direction / Isometry.Scale);
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
            obstacles = obstacles.Where(unit => unit.Movement == null || ! unit.Movement.Displaceable).ToArray();
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