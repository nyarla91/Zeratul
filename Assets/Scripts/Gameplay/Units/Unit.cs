using System;
using Gameplay.Data;
using Gameplay.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitOrders))]
    [RequireComponent(typeof(UnitSight))]
    [RequireComponent(typeof(UnitVisibility))]
    [RequireComponent(typeof(UnitStatuses))]
    [RequireComponent(typeof(UnitStagger))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitAttackConfig _unitAttackConfig;
        
        private UnitMovement _movement;
        private UnitOrders _orders;
        private UnitSight _sight;
        private UnitVisibility _visibility;

        public UnitAbilities Abilities { get; private set; }
        public UnitAttack Attack { get; private set; }
        public UnitLife Life { get; private set; }
        public UnitOwnership Ownership { get; private set; }
        public UnitStatuses Statuses { get; private set; }
        public UnitStagger Stagger { get; private set; }
        public UnitMovement Movement => _movement ??= GetComponent<UnitMovement>();
        public UnitOrders Orders => _orders ??= GetComponent<UnitOrders>();
        public UnitSight Sight => _sight ??= GetComponent<UnitSight>();
        public UnitVisibility Visibility => _visibility ??= GetComponent<UnitVisibility>();

        public Vector2 Position => transform.position;

        public UnitType Type { get; private set; }

        public event Action Killed; 
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        
        public void Init(UnitType type, bool ownedByPlayer)
        {
            Type = type;

            Abilities = new UnitAbilities(this, TacticalPause);
            Attack = new UnitAttack(this, TacticalPause, _unitAttackConfig);
            Life = new UnitLife(this, TacticalPause);
            Ownership = new UnitOwnership(this, ownedByPlayer);
            Stagger = new UnitStagger(this, TacticalPause);
            Statuses = new UnitStatuses(this, TacticalPause);
            
            Movement.Init(type);
            Orders.Init(type);
            Sight.Init(type, ownedByPlayer);
            
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