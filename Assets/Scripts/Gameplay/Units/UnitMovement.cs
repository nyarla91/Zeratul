using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Map;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

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
        public bool Displaceable => Unit.Orders.IsIdle;
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
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => MoveAlongPath());
            
            Unit.FixedUpdateAsObservable()
                .Subscribe(_ => UpdatePhysics());
        }

        public void Move(Vector2 destination, float desiredDistance = 0)
        {
            if (UnitType.IsImmobile || HasPath && Time.time < _lastPathRecalculationTime + _config.MinPathRecalculationPeriod)
                return;

            if (desiredDistance > 0)
            {
                _nodeMap.CanPassBetween(Unit.Position, destination, UnitType.PathfindingAgent, out RaycastHit2D hit);
                if ((Isometry.Distance(hit.point, destination)) < desiredDistance)
                {
                    destination = hit.point;
                }
            }
                
            _nodeMap.TryFindPath(Unit.Position, destination, out _path, UnitType.PathfindingAgent);
            if (_path == null || _path.Count == 0 || HasReachedPoint(_path.Last(), true))
            {
                Stop();
                return;
            }
            _lastPathRecalculationTime = Time.time;
        }

        public void Teleport(Vector2 position)
        {
            Unit.transform.position = position;
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

            if (_tacticalPause.IsPaused)
                _rigidbody.linearVelocity = Vector2.zero;
        }

        private void MoveAlongPath()
        {
            if ( ! HasPath || Unit.Stagger.IsStaggered)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                return;
            }
            
            Vector2 direction = Unit.Position.DirectionTo(_path.First());
            direction = AvoidObstaclesForDirection(direction, out bool corrected);
            
            if (HasReachedPoint(_path.First(), ! corrected))
                _path.RemoveAt(0);
            
            if (_path.Count == 0)
            {
                Stop();
                return;
            }

            float speed = Speed * Mathf.Lerp(1, Isometry.VerticalScale, Mathf.Abs(direction.y));
            Unit.Direction.RotateTowards(direction / Isometry.Scale);
            _rigidbody.linearVelocity = speed * direction;
        }

        public bool HasReachedPoint(Vector2 point, bool exact)
        {
            float tolerance = _config.NodeProximityDistance;
            if (!exact)
                tolerance += UnitType.Size / 2;
            return point.OrthogonalDistance(Unit.Position) < tolerance;
        }

        private Vector2 AvoidObstaclesForDirection(Vector2 direction, out bool corrected)
        {
            direction.Normalize();
            corrected = false;
            
            ContactFilter2D contactFilter = new()
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = UnitType.IsAir
                    ? (_config.AirMask | _config.CommonObstacleMask)
                    : (_config.GroundMask | _config.GroundObstacleMask)
            };

            List<RaycastHit2D> results = new();
            if (_avoidanceCollider.Cast(direction, contactFilter, results, _config.AvoidanceDistance) == 0)
                return direction;
            
            RaycastHit2D hit = results[0];
            if ( ! IsColliderAvoidable(hit.collider))
                return direction;
            
            Vector2 directionToObstacle = hit.point - Unit.Position;
            directionToObstacle.Normalize();

            /*int closestObtacleSideSign = 0;
            float angleToClosestObstacle = 180;
            Vector2 directionToClosestObstacle = Vector2.zero;
            
            foreach (Collider2D collider in overlap)
            {
                if (collider == null || ! IsColliderAvoidable(collider))
                    continue;
                Vector2 contactPoint = collider.ClosestPoint(Unit.Position);

                Vector2 a = Unit.Position;
                Vector2 b = a + direction;
                float side =
                    (b.x - a.x) * (contactPoint.y - a.y) -
                    (b.y - a.y) * (contactPoint.x - a.x);
                int sideSign = side.Sign();
                Debug.Log($"Sign {sideSign}");
                if (closestObtacleSideSign != 0 && closestObtacleSideSign != sideSign)
                    return direction;
                closestObtacleSideSign = sideSign;

                Vector2 directionToObstacle = Unit.Position.DirectionTo(contactPoint);
                float angle = Vector2.Angle(direction, contactPoint);
                if (angle > angleToClosestObstacle || angle > _config.AvoidanceArc / 2)
                    continue;

                directionToClosestObstacle = directionToObstacle;
                angleToClosestObstacle = angle;
            }*/
            
            Vector2 correctedDirection = direction - directionToObstacle;
            correctedDirection.Normalize();
            corrected = true;
            return Vector2.Lerp(direction,  correctedDirection, _config.AvoidanceStrength).normalized;
        }

        private bool IsColliderAvoidable(Collider2D collider)
        {
            LayerMask mask = (_config.CommonObstacleMask | _config.GroundObstacleMask);
            if (mask.Includes(collider.gameObject.layer))
                return true;
            Unit unit = collider.GetComponentInParent<Unit>();
            if ( ! unit)
                return false;
            if ( ! unit.CanMove)
                return true;
            if (unit.Orders.IsIdle)
                return false;
            float deltaVelocity = Vector2.Angle(Velocity, unit.Movement.Velocity);
            return deltaVelocity > 60;
        }
    }
}