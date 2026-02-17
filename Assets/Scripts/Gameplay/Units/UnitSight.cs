using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data;
using Gameplay.Data.Configs;
using Gameplay.Data.Validator;
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
            
            AttachSightArea(ownedByPlayer);
            Unit.Ownership.OwnerUpdated += AttachSightArea;
            
            _area.compositeOperation = Collider2D.CompositeOperation.Merge;
        }

        public HashSet<Unit> VisibleUnits(UnitValidatorGroup validatorGroup = default)
        {
            HashSet<Unit> result = VisionMap.GetAreaForOwner(Unit.Ownership.OwnedByPlayer).VisibleUnits;
            result = result.Where(u => Isometry.Distance(Unit.Position, u.Position) < Radius).ToHashSet();
            
            return result.Where(u => validatorGroup.IsValid(Unit, u)).ToHashSet();
        }

        private void AttachSightArea(bool ownedByPlayer)
        {
            VisionMap.GetAreaForOwner(ownedByPlayer).AttachSightArea(_area.transform);
        }

        private void Recalculate()
        {
            _area.transform.position = Unit.Position;

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
                    RaycastHit2D raycast = Physics2D.Raycast(Unit.Position, direction, maxDistance, _config.VisionBlockerMask);
                    point = raycast.collider ? (raycast.point - Unit.Position) : direction * maxDistance;
                    point += direction * _config.AbsoluteExtraSight;
                }
                float minDistance = _config.MinSight + UnitType.Size / 2;
                minDistance = Isometry.DistanceTowards(minDistance, direction.y);
                if (point.magnitude < minDistance)
                    point = direction * minDistance;
                points[i] =  point;
            }
            _area.points = points;
        }

        private void OnDestroy()
        {
            VisionMap.RecalculationTimer.Expired -= Recalculate;
            if (_area)
                Destroy(_area.gameObject);
        }
    }
}