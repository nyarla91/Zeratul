using System;
using Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    [RequireComponent(typeof(UnitOwnership))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitOrders))]
    [RequireComponent(typeof(UnitAttack))]
    [RequireComponent(typeof(UnitLife))]
    [RequireComponent(typeof(UnitSight))]
    [RequireComponent(typeof(UnitVisibility))]
    [RequireComponent(typeof(UnitStatuses))]
    [RequireComponent(typeof(UnitStagger))]
    public class Unit : MonoBehaviour
    {
        private UnitOwnership _ownership;
        private UnitMovement _movement;
        private UnitOrders _orders;
        private UnitAttack _attack;
        private UnitLife _life;
        private UnitSight _sight;
        private UnitVisibility _visibility;
        private UnitStatuses _statuses;
        private UnitStagger _stagger;

        public UnitAbilities Abilities { get; private set; }
        public UnitOwnership Ownership => _ownership ??= GetComponent<UnitOwnership>();
        public UnitMovement Movement => _movement ??= GetComponent<UnitMovement>();
        public UnitOrders Orders => _orders ??= GetComponent<UnitOrders>();
        public UnitAttack Attack => _attack ??= GetComponent<UnitAttack>();
        public UnitLife Life => _life ??= GetComponent<UnitLife>();
        public UnitSight Sight => _sight ??= GetComponent<UnitSight>();
        public UnitVisibility Visibility => _visibility ??= GetComponent<UnitVisibility>();
        public UnitStatuses Statuses => _statuses ??= GetComponent<UnitStatuses>();
        public UnitStagger Stagger => _stagger ??= GetComponent<UnitStagger>();

        public Vector2 Position => transform.position;

        public UnitType Type { get; private set; }

        public event Action Killed; 
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        
        public void Init(UnitType type, bool ownedByPlayer)
        {
            Type = type;

            Abilities = new UnitAbilities(this, TacticalPause);
            
            Attack.Init(type);
            Ownership.Init(type, ownedByPlayer);
            Movement.Init(type);
            Orders.Init(type);
            Life.Init(type);
            Sight.Init(type, ownedByPlayer);
            Statuses.Init(type);
            
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