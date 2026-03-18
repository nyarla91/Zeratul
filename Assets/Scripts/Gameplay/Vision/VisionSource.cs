using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Vision
{
    public class VisionSource : MonoBehaviour
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private PolygonCollider2D _collider;
        
        private bool _isAir;
        private Transform _anchor;
        private bool _ownedByPlayer;

        public float Radius { get; set; }
        
        public bool OwnedByPlayer
        {
            get => _ownedByPlayer;
            set
            {
                if (_ownedByPlayer == value)
                    return;
                _ownedByPlayer = value;
                AttachToArea(_ownedByPlayer);
            }
        }
        
        [Inject] private VisionMap VisionMap { get; set; }

        public void Set(Transform anchor, bool isAir, float radius, bool ownedByPlayer)
        {
            _anchor = anchor;
            _isAir = isAir;
            Radius = radius;
            OwnedByPlayer = ownedByPlayer;
            _collider.compositeOperation = Collider2D.CompositeOperation.Merge;
            AttachToArea(ownedByPlayer);
        }
        
        public void Recalculate()
        {
            _collider.transform.position = _anchor.transform.position;
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
                if (_isAir)
                {
                    point = direction * (maxDistance + _config.AbsoluteExtraSight);
                }
                else
                {
                    RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, maxDistance, _config.VisionBlockerMask);
                    point = raycast.collider ? (raycast.point - (Vector2) transform.position) : direction * maxDistance;
                    point += direction * _config.AbsoluteExtraSight;
                }
                float minDistance = _config.MinSight;
                minDistance = Isometry.DistanceTowards(minDistance, direction.y);
                if (point.magnitude < minDistance)
                    point = direction * minDistance;
                points[i] =  point;
            }

            _collider.points = points;
        }
        
        public HashSet<Unit> VisibleUnits(Unit host = null, UnitValidatorGroup validatorGroup = default)
        {
            HashSet<Unit> result = VisionMap.GetAreaForOwner(OwnedByPlayer).VisibleUnits;
            result = result.Where(u => Isometry.Distance(transform.position, u.Position) < Radius).ToHashSet();
            
            return result.Where(u => validatorGroup.IsValid(host, u)).ToHashSet();
        }

        public void Dispose()
        {
            DetachFromAll();
            Destroy(gameObject);
        }

        private void AttachToArea(bool ownedByPlayer)
        {
            DetachFromAll();
            VisionMap.GetAreaForOwner(ownedByPlayer).AttachSource(this);
        }

        private void DetachFromAll()
        {
            VisionMap.PlayerArea.DetachSource(this);
            VisionMap.EnemyArea.DetachSource(this);
        }
    }
}