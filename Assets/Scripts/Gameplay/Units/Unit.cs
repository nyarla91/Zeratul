using System;
using System.Linq;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Data.Units;
using Gameplay.Map;
using Gameplay.Player;
using Gameplay.Vision;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitAttackConfig _unitAttackConfig;
        [SerializeField] private UnitMovementConfig _unitMovementConfig;
        [SerializeField] private PathfindingConfig _pathfindingConfig;
        [SerializeField] private OrderErrorConfig _orderErrorConfig;
        [SerializeField] private VisionConfig _visionConfig;
        [SerializeField] private UnitAiConfig _aiConfig;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _avoidanceCollider;
        [SerializeField] private Collider2D _simulationCollider;
        [SerializeField] private Collider2D _obstacleCollider;
        [SerializeField] private BoxCollider2D _interactionCollider;
        [SerializeField] private VisionSource _visionSource;
        
        public UnitDirection Direction { get; private set; }
        public UnitAbilities Abilities { get; private set; }
        public UnitLife Life { get; private set; }
        public UnitOwnership Ownership { get; private set; }
        public UnitStatuses Statuses { get; private set; }
        public UnitStagger Stagger { get; private set; }
        public UnitVisibility Visibility { get; private set; }
        public UnitSight Sight { get; private set; }
        public UnitOrders Orders { get; private set; }
        public UnitAttack Attack { get; private set; }
        public UnitMovement Movement { get; private set; }
        public UnitAI AI { get; private set; }
        public UnitSimulation Simulation { get; private set; }
        public UnitPathing Pathing { get; private set; }

        [ShowNativeProperty] public bool CanAttack => Type.WeaponType;
        [ShowNativeProperty] public bool CanMove => ! Type.IsImmobile;
        [ShowNativeProperty] public bool IsVisibleToPlayer => Visibility?.IsVisibleToPlayer ?? false;

        public bool IsDead { get; private set; }
        public bool IsAlive => ! IsDead;
        
        public Vector2 Position => transform ? transform.position : Vector2.zero;
        public Vector2 InteractionPosition => _interactionCollider.transform.position + (Vector3) _interactionCollider.offset;
        
        public bool IsHighlighted => MouseTargeting.Unit == this;
        public bool IsSelected => Selection.IsUnitSelected(this);
        
        public UnitType Type { get; private set; }

        public event Action Killed; 
        public int Id { get; set; }
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private GameTime GameTime { get; set; }
        [Inject] private NodeMap NodeMap { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private VisionMap VisionMap { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }

        public void Init(int id, UnitSpawnInfo spawnInfo)
        {
            if (Type != null)
                return;

            Id = id;
            Type = spawnInfo.UnitType;

            Direction = new UnitDirection(this, TacticalPause, spawnInfo.LookAngle);
            Abilities = new UnitAbilities(this, TacticalPause);
            Life = new UnitLife(this, TacticalPause);
            Ownership = new UnitOwnership(this, spawnInfo.OwnedByPlayer);
            Stagger = new UnitStagger(this, TacticalPause);
            Visibility = new UnitVisibility(this, VisionMap);
            Sight = new UnitSight(this, _visionConfig, _visionSource, VisionMap, spawnInfo.OwnedByPlayer);
            Orders = new UnitOrders(this, TacticalPause);
            Statuses = new UnitStatuses(this, TacticalPause);
            AI = new UnitAI(this, TacticalPause, GameTime, _aiConfig, spawnInfo.PatrolPath);
            Simulation = new UnitSimulation(this, TacticalPause, _visionConfig, _simulationCollider);
            Pathing = new UnitPathing(this, _pathfindingConfig, _unitMovementConfig, NodeMap, _rigidbody, _obstacleCollider, _collider);
            
            if (CanAttack)
                Attack = new UnitAttack(this, TacticalPause, _unitAttackConfig, _orderErrorConfig);
            
            if (CanMove)
                Movement = new UnitMovement(this, TacticalPause, NodeMap, _unitMovementConfig, _rigidbody, _avoidanceCollider);
            
            UnitPool.AddUnit(this);
        }

        public void Kill()
        {
            IsDead = true;
            UnitPool.RemoveUnit(this);
            Killed?.Invoke();
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (IsAlive)
                Killed?.Invoke();
        }
    }
}