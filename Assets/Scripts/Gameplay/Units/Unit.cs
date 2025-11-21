using UnityEngine;

namespace Gameplay.Units
{
    [RequireComponent(typeof(UnitOwnership))]
    public class Unit : MonoBehaviour
    {
        private UnitOwnership _ownership;

        public UnitOwnership Ownership => _ownership ??= GetComponent<UnitOwnership>();
    }
}