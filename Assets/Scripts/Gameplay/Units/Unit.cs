using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Units
{
    [RequireComponent(typeof(UnitOwnership))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitOrders))]
    [RequireComponent(typeof(UnitAttack))]
    [RequireComponent(typeof(UnitLife))]
    [RequireComponent(typeof(UnitSight))]
    public class Unit : MonoBehaviour
    {
        private UnitOwnership _ownership;
        private UnitMovement _movement;
        private UnitOrders _orders;
        private UnitAttack _attack;
        private UnitLife _life;
        private UnitSight _sight;

        public UnitOwnership Ownership => _ownership ??= GetComponent<UnitOwnership>();
        public UnitMovement Movement => _movement ??= GetComponent<UnitMovement>();
        public UnitOrders Orders => _orders ??= GetComponent<UnitOrders>();
        public UnitAttack Attack => _attack ??= GetComponent<UnitAttack>();
        public UnitLife Life => _life ??= GetComponent<UnitLife>();
        public UnitSight Sight => _sight ??= GetComponent<UnitSight>();

        public UnitType Type { get; private set; }
        
        public void Init(UnitType type, bool ownedByPlayer)
        {
            Type = type;
            
            Ownership.Init(type, ownedByPlayer);
            Movement.Init(type);
            Orders.Init(type);
            Attack.Init(type);
            Life.Init(type);
            Sight.Init(type, ownedByPlayer);
            
            Life.OnHitPointsOver += Die;
        }
        
        private void Die()
        {
            Destroy(gameObject);
        }
    }
}