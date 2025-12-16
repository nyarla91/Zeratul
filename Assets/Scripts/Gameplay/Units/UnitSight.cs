using Gameplay.Data;
using Gameplay.Vision;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitSight : UnitComponent
    {
        [SerializeField] private PolygonCollider2D _sightArea;
        
        [Inject] private VisionMap VisionMap { get; set; }
        
        public void Init(UnitType unitType, bool ownedByPlayer)
        {
            _sightArea.transform.localScale = unitType.SightRadius * 2 * Vector3.one;
            VisionMap.AttachSightArea(_sightArea.transform, ownedByPlayer);
        }

        private void FixedUpdate()
        {
            _sightArea.transform.position = transform.position;
        }

        private void OnDestroy()
        {
            Destroy(_sightArea.gameObject);
        }
    }
}