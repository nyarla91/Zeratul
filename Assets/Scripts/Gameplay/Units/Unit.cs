using System;
using System.Linq;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Data.Units;
using Gameplay.Pathfinding;
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
        [SerializeField] private OrderErrorConfig _orderErrorConfig;
        [SerializeField] private VisionConfig _visionConfig;
        [SerializeField] private UnitAiConfig _aiConfig;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _avoidanceCollider;
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

        [ShowNativeProperty] public bool CanAttack { get; private set; }
        [ShowNativeProperty] public bool CanMove { get; private set; }
        [ShowNativeProperty] public bool IsVisibleToPlayer => Visibility?.IsVisibleToPlayer ?? false;

        public Vector2 Position => transform.position;
        public Vector2 InteractionPosition => _interactionCollider.transform.position + (Vector3) _interactionCollider.offset;

        public UnitType Type { get; private set; }

        public event Action Killed; 
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private NodeMap NodeMap { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private VisionMap VisionMap { get; set; }
        
        public void Init(UnitType type, bool ownedByPlayer)
        {
            if (Type != null)
                return;
            
            Type = type;

            Direction = new UnitDirection(this, TacticalPause);
            Abilities = new UnitAbilities(this, TacticalPause);
            Life = new UnitLife(this, TacticalPause);
            Ownership = new UnitOwnership(this, ownedByPlayer);
            Stagger = new UnitStagger(this, TacticalPause);
            Visibility = new UnitVisibility(this, VisionMap);
            Sight = new UnitSight(this, _visionConfig, _visionSource, VisionMap, ownedByPlayer);
            Orders = new UnitOrders(this, TacticalPause);
            Statuses = new UnitStatuses(this, TacticalPause);
            AI = new UnitAI(this, _aiConfig);
            
            CanAttack = type.WeaponType != null && type.AvailableOrders.Contains(_unitAttackConfig.DefaultAttackOrder);
            if (CanAttack)
                Attack = new UnitAttack(this, TacticalPause, _unitAttackConfig, _orderErrorConfig);
            
            CanMove = ! type.IsImmobile;
            if (CanMove)
                Movement = new UnitMovement(this, TacticalPause, NodeMap, _unitMovementConfig, _rigidbody, _avoidanceCollider);
            else
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            
            _collider.transform.localScale = Vector3.one * Type.Size;
            _collider.gameObject.layer = gameObject.layer;
            gameObject.layer = Type.IsAir ? _unitMovementConfig.AirLayer : _unitMovementConfig.GroundLayer;
            
            UnitPool.AddUnit(this);
            Life.HitPointsOver += Kill;
        }
        
        public void Kill()
        {
            Killed?.Invoke();
            UnitPool.RemoveUnit(this);
            Destroy(gameObject);
        }
    }
}