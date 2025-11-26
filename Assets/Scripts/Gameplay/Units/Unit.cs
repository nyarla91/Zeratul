using GaameplayData.Units;
using UnityEngine;

namespace Gameplay.Units
{
    [RequireComponent(typeof(UnitOwnership))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitType _type;

        public UnitType Type => _type;

        private UnitOwnership _ownership;
        private UnitMovement _movement;
        private UnitOrders _orders;

        public UnitOwnership Ownership => _ownership ??= GetComponent<UnitOwnership>();
        public UnitMovement Movement => _movement ??= GetComponent<UnitMovement>();
        public UnitOrders Orders => _orders ??= GetComponent<UnitOrders>();
    }
}