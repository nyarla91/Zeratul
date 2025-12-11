using System;
using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Units
{
    [RequireComponent(typeof(UnitOwnership))]
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(UnitOrders))]
    [RequireComponent(typeof(UnitAttack))]
    [RequireComponent(typeof(UnitLife))]
    [RequireComponent(typeof(UnitGraphics))]
    [RequireComponent(typeof(UnitVIsion))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitType _type;

        public UnitType Type => _type;

        private UnitOwnership _ownership;
        private UnitMovement _movement;
        private UnitOrders _orders;
        private UnitAttack _attack;
        private UnitLife _life;
        private UnitGraphics _graphics;
        private UnitVIsion _vision;

        public UnitOwnership Ownership => _ownership ??= GetComponent<UnitOwnership>();
        public UnitMovement Movement => _movement ??= GetComponent<UnitMovement>();
        public UnitOrders Orders => _orders ??= GetComponent<UnitOrders>();
        public UnitAttack Attack => _attack ??= GetComponent<UnitAttack>();
        public UnitLife Life => _life ??= GetComponent<UnitLife>();
        public UnitGraphics Graphics => _graphics ??= GetComponent<UnitGraphics>();
        public UnitVIsion Vision => _vision ??= GetComponent<UnitVIsion>();

        private void Awake()
        {
            Life.OnHitPointsOver += Die;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}