using Extentions;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Vision;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitSight : UnitComponent
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private PolygonCollider2D _area;
        [SerializeField] private int _areaPoints;
        
        [Inject] private VisionMap VisionMap { get; set; }

        private Modifier _radiusModifier;

        public Modifier RadiusModifier => _radiusModifier;
        public float Radius => UnitType.SightRadius * RadiusModifier.Value;
        
        public void Init(UnitType unitType, bool ownedByPlayer)
        {
            VisionMap.RecalculationTimer.Expired += Recalculate;

            _radiusModifier = new Modifier();
            
            if (ownedByPlayer)
                VisionMap.PlayerArea.AttachSightArea(_area.transform);
            else
                VisionMap.EnemyArea.AttachSightArea(_area.transform);
            _area.compositeOperation = Collider2D.CompositeOperation.Merge;
        }

        private void Recalculate()
        {
            _area.transform.position = transform.position;

            Vector2[] points =  new Vector2[_areaPoints];
            
            for (int i = 0; i < _areaPoints; i++)
            {
                float angle = 360 / (float) _areaPoints * i;
                Vector2 direction = angle.DegreesToVector2();
                direction.Normalize();
                float maxDistance = Radius;
                maxDistance *= Mathf.Lerp(1, Isometry.VerticalScale, Mathf.Abs(direction.y));
                Vector2 point;
                if (UnitType.IsAir)
                {
                    point = direction * (maxDistance + _config.AbsoluteExtraSight);
                }
                else
                {
                    RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, maxDistance, _config.VisionBlockerMask);
                    point = raycast.collider ? (raycast.point - (Vector2)transform.position) : direction * maxDistance;
                    point += direction * _config.AbsoluteExtraSight;
                }
                points[i] =  point;
            }
            _area.points = points;
        }

        private void OnDestroy()
        {
            VisionMap.RecalculationTimer.Expired -= Recalculate;
            _area?.gameObject.Destroy();
        }
    }
}