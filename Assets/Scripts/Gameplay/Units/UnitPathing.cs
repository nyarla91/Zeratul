using Gameplay.Data.Configs;
using Gameplay.Map;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitPathing : UnitComponent
    {
        private readonly PathfindingConfig _pathfindingConfig;
        private readonly NodeMap _nodeMap;
        private readonly Collider2D _obstacleCollider;

        private Bounds RecalculationBounds => new(Unit.Position,
            Isometry.Scale * UnitType.Size + _pathfindingConfig.MaxObstacleDistance * Vector2.one);

        public UnitPathing(Unit unit, PathfindingConfig pathfindingConfig, UnitMovementConfig movementConfig,
            NodeMap nodeMap, Rigidbody2D rigidbody, Collider2D obstacleCollider, Collider2D collider) : base(unit)
        {
            _pathfindingConfig = pathfindingConfig;
            _nodeMap = nodeMap;
            _obstacleCollider = obstacleCollider;

            obstacleCollider.enabled = ! Unit.CanMove;
            collider.transform.localScale = Vector3.one * UnitType.Size;
            collider.gameObject.layer = Unit.gameObject.layer;
            collider.isTrigger = UnitType.NoCollision;
            Unit.gameObject.layer = UnitType.IsAir ? movementConfig.AirLayer : movementConfig.GroundLayer;

            if (Unit.CanMove)
                return;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            _nodeMap.QueueObstacleRecalculation(RecalculationBounds);
            Unit.Killed += DisposeObstacle;
        }
        
        public void DisposeObstacle()
        {
            _nodeMap.QueueObstacleRecalculation(RecalculationBounds);
            _obstacleCollider.enabled = false;
        }
    }
}