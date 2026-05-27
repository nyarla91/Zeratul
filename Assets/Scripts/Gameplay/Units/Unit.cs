using System;
using Extentions;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Data.Units;
using Gameplay.Map;
using Gameplay.Player;
using Gameplay.Vision;
using NaughtyAttributes;
using Save.Data.Units;
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
        [SerializeField] private Collider2D _obstacleCollider;
        [SerializeField] private BoxCollider2D _interactionCollider;
        
        public UnitDirection Direction { get; private set; }
        public UnitAbilities Abilities { get; private set; }
        public UnitLife Life { get; private set; }
        public UnitAlliance Alliance { get; private set; }
        public UnitStatuses Statuses { get; private set; }
        public UnitStagger Stagger { get; private set; }
        public UnitVisibility Visibility { get; private set; }
        public UnitSight Sight { get; private set; }
        public UnitOrders Orders { get; private set; }
        public UnitAttack Attack { get; private set; }
        public UnitMovement Movement { get; private set; }
        public UnitAI AI { get; private set; }
        public UnitPathing Pathing { get; private set; }

        [ShowNativeProperty] public bool CanAttack => Type.WeaponType;
        [ShowNativeProperty] public bool CanMove => ! Type.IsImmobile;
        [ShowNativeProperty] public bool IsVisibleToPlayer => Visibility?.IsVisibleTo(Owner.Player) ?? false;

        public bool IsDead { get; private set; }
        public bool IsAlive => ! IsDead;
        
        public Vector2 Position => transform ? transform.position : Vector2.zero;
        public Vector2 InteractionPosition => _interactionCollider.transform.position + (Vector3) _interactionCollider.offset;
        
        public bool IsHighlighted => MouseTargeting.Unit == this;
        public bool IsSelected => Selection.IsUnitSelected(this);
        public bool IsSimulated => VisionMap.IsPointSimulated(Position);
        
        public UnitType Type { get; private set; }

        public event Action Killed; 
        public int Id { get; private set; }
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private GameTime GameTime { get; set; }
        [Inject] private NodeMap NodeMap { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private VisionMap VisionMap { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }
        [Inject] private GameDataRegistry GameDataRegistry { get; set; }

        public void Init(int id, UnitType type, UnitSpawnInfo spawnInfo = null)
        {
            if (Type != null)
                return;

            Id = id;
            Type = type;

            Direction = new UnitDirection(this, TacticalPause, spawnInfo?.LookAngle ?? 0);
            Abilities = new UnitAbilities(this, GameTime, TacticalPause, GameDataRegistry);
            Life = new UnitLife(this, GameTime, TacticalPause, UnitPool);
            Alliance = new UnitAlliance(this, spawnInfo?.Owner ?? Owner.Enemy);
            Stagger = new UnitStagger(this, TacticalPause);
            Visibility = new UnitVisibility(this, VisionMap);
            Sight = new UnitSight(this, VisionMap);
            Orders = new UnitOrders(this, TacticalPause, GameDataRegistry, UnitPool);
            Statuses = new UnitStatuses(this, GameTime, TacticalPause, GameDataRegistry, UnitPool);
            AI = new UnitAI(this, TacticalPause, GameTime, _aiConfig, spawnInfo?.PatrolPath);
            Pathing = new UnitPathing(this, _pathfindingConfig, _unitMovementConfig, NodeMap, _rigidbody, _obstacleCollider, _collider);
            
            if (CanAttack)
                Attack = new UnitAttack(this, TacticalPause, _unitAttackConfig, _orderErrorConfig);
            
            if (CanMove)
                Movement = new UnitMovement(this, TacticalPause, NodeMap, _unitMovementConfig, _rigidbody, _avoidanceCollider);
            
            UnitPool.AddUnit(this);
        }

        public UnitSaveData Save()
        {   
            return new UnitSaveData(new[]
            {
                Direction.Save(),
                Abilities.Save(),
                Life.Save(),
                Alliance.Save(),
                Stagger.Save(),
                Visibility.Save(),
                Sight.Save(),
                Orders.Save(),
                Statuses.Save(),
                AI.Save(),
                Pathing.Save(),
                Attack?.Save(),
                Movement?.Save()
            }, Id, Type.name, Position);
        }

        public void ReproduceFromSave(UnitSaveData saveData)
        {
            Direction.ReproduceFromSave(saveData);
            Abilities.ReproduceFromSave(saveData);
            Life.ReproduceFromSave(saveData);
            Alliance.ReproduceFromSave(saveData);
            Stagger.ReproduceFromSave(saveData);
            Visibility.ReproduceFromSave(saveData);
            Sight.ReproduceFromSave(saveData);
            Statuses.ReproduceFromSave(saveData);
            Orders.ReproduceFromSave(saveData);
            AI.ReproduceFromSave(saveData);
            Pathing.ReproduceFromSave(saveData);
            Attack?.ReproduceFromSave(saveData);
            Movement?.ReproduceFromSave(saveData);
        }

        public bool GetFlag(UnitFlag flag)
        {
            return flag switch
            {
                UnitFlag.IsAir => Type.IsAir,
                UnitFlag.CanMove => CanMove,
                UnitFlag.CanAttack => CanAttack,
                UnitFlag.IsHighlighted => IsHighlighted,
                UnitFlag.IsSelected => IsSelected,
                UnitFlag.IsLocked => Abilities.IsLocked,
                UnitFlag.HasEnergy => Abilities.HasEnergyPoints,
                UnitFlag.HasShields => Life.HasShieldPoints,
                UnitFlag.IsStaggered => Stagger.IsStaggered,
                UnitFlag.IsCloaked => Visibility.IsCloaked,
                UnitFlag.IsDetected => Visibility.IsDetected,
                UnitFlag.IsIdle => Orders.IsIdle,
                _ => throw new ArgumentOutOfRangeException(nameof(flag), flag, null)
            };
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