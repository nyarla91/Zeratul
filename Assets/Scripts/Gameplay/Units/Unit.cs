using System;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Pathfinding;
using Gameplay.Vision;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitAttackConfig _unitAttackConfig;
        [SerializeField] private UnitMovementConfig _unitMovementConfig;
        [SerializeField] private VisionConfig _visionConfig;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _avoidanceCollider;
        [SerializeField] private PolygonCollider2D _visionArea;
        
        private UnitOrders _orders;

        public UnitAbilities Abilities { get; private set; }
        public UnitAttack Attack { get; private set; }
        public UnitLife Life { get; private set; }
        public UnitOwnership Ownership { get; private set; }
        public UnitStatuses Statuses { get; private set; }
        public UnitStagger Stagger { get; private set; }
        public UnitMovement Movement { get; private set; }
        public UnitVisibility Visibility { get; private set; }
        public UnitSight Sight { get; private set; }
        public UnitOrders Orders => _orders ??= GetComponent<UnitOrders>();

        public Vector2 Position => transform.position;

        public UnitType Type { get; private set; }

        public event Action Killed; 
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private NodeMap NodeMap { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private VisionMap VisionMap { get; set; }
        
        public void Init(UnitType type, bool ownedByPlayer)
        {
            Type = type;

            Abilities = new UnitAbilities(this, TacticalPause);
            Attack = new UnitAttack(this, TacticalPause, _unitAttackConfig);
            Life = new UnitLife(this, TacticalPause);
            Ownership = new UnitOwnership(this, ownedByPlayer);
            Stagger = new UnitStagger(this, TacticalPause);
            Statuses = new UnitStatuses(this, TacticalPause);
            Movement = new UnitMovement(this, TacticalPause, NodeMap, _unitMovementConfig, _rigidbody, _collider, _avoidanceCollider);
            Visibility = new UnitVisibility(this, VisionMap);
            Sight = new UnitSight(this, _visionConfig, _visionArea, VisionMap, ownedByPlayer);
            
            Orders.Init(type);
            
            UnitPool.AddUnit(this);
            Life.HitPointsOver += Kill;

            Debug.Log("Ferwer");
        }
        
        public void Kill()
        {
            Killed?.Invoke();
            UnitPool.RemoveUnit(this);
            Destroy(gameObject);
        }
    }
}