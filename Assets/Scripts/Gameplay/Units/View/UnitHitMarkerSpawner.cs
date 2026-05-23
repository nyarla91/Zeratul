using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitHitMarkerSpawner : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        
        [Inject] private PoolFactory<HitMarker> HitMarkerFactory { get; set; }

        private void Start()
        {
            if (_unit.CanAttack)
                _unit.Attack.Struck += SpawnHitMarker;
        }

        private void SpawnHitMarker(Unit target)
        {
            HitMarker hitMarker = HitMarkerFactory.Get();
            hitMarker.InitHit(_unit, target);
        }
    }
}