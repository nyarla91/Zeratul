using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Data.Validator;
using Gameplay.Vision;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Units
{
    public class UnitSight : UnitComponent
    {
        private readonly VisionConfig _config;
        private readonly VisionMap _visionMap;
        private readonly PolygonCollider2D _area;

        public Modifier RadiusModifier { get; } = new Modifier();
        public float Radius => UnitType.SightRadius * RadiusModifier.Value;

        public UnitSight(Unit unit, VisionConfig config, PolygonCollider2D area, VisionMap visionMap, bool ownedByPlayer) : base(unit)
        {
            _config = config;
            _area = area;
            _visionMap = visionMap;

            Observable.EveryFixedUpdate()
                .Sample(TimeSpan.FromSeconds(_config.RecalculationPeriod))
                .Subscribe(_ => Recalculate());
            
            AttachSightArea(ownedByPlayer);
            
            Unit.Ownership.ObserveEveryValueChanged(o => o.OwnedByPlayer)
                .Subscribe(a => AttachSightArea(Unit.Ownership.OwnedByPlayer));
            
            _area.compositeOperation = Collider2D.CompositeOperation.Merge;

            Unit.Killed += DestroyArea;
        }

        public HashSet<Unit> VisibleUnits(UnitValidatorGroup validatorGroup = default)
        {
            HashSet<Unit> result = _visionMap.GetAreaForOwner(Unit.Ownership.OwnedByPlayer).VisibleUnits;
            result = result.Where(u => Isometry.Distance(Unit.Position, u.Position) < Radius).ToHashSet();
            
            return result.Where(u => validatorGroup.IsValid(Unit, u)).ToHashSet();
        }

        private void AttachSightArea(bool ownedByPlayer)
        {
            _visionMap.GetAreaForOwner(ownedByPlayer).AttachSightArea(_area.transform);
        }

        private void Recalculate()
        {
            if ( ! _area)
                return;
            _area.transform.position = Unit.Position;
            int areaPoints = _config.UnitVisionPoints;

            Vector2[] points =  new Vector2[areaPoints];
            
            for (int i = 0; i < areaPoints; i++)
            {
                float angle = 360 / (float) areaPoints * i;
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

        private void DestroyArea()
        {
            if (_area)
                Object.Destroy(_area.gameObject);
        }
    }
}