using Extentions;
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
            _sightArea.transform.localScale = unitType.General.SightRadius * 2 * Vector3.one;
            if (ownedByPlayer)
                VisionMap.PlayerArea.AttachSightArea(_sightArea.transform);
            else
                VisionMap.EnemyArea.AttachSightArea(_sightArea.transform);
            _sightArea.compositeOperation = Collider2D.CompositeOperation.Merge;
        }

        private void FixedUpdate()
        {
            _sightArea.transform.position = transform.position;
        }

        private void OnDestroy()
        {
            _sightArea?.gameObject.Destroy();
        }
    }
}