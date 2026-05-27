using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Vision
{
    public class VisionSource
    {
        private readonly VisionMap _visionMap;
        private readonly IsometricOverlap _isometricOverlap;
        private readonly VisionConfig _config;

        private VisionResult _visionResult;
        private HashSet<Unit> _visibleUnits = new();
        private Func<Vector3> _position;
        private Func<float> _radius;
        private Func<Owner> _owner;
        private Func<bool> _isAir;

        public HashSet<Unit> VisibleUnits => _visibleUnits
            .Where(u => u.Visibility.IsRevealed || u.Alliance.IsFriendly(Owner))
            .ToHashSet();

        public Vector3 Position => _position.Invoke();
        public float Radius => _radius.Invoke();
        public Owner Owner => _owner.Invoke();
        public bool IsAir => _isAir.Invoke();
        
        public Bounds Bounds => new(Position, Radius * 2 * Isometry.Scale);
        public Bounds SimulationBounds => new(Position, Radius * 2 * Isometry.Scale + Vector2.one * _config.SimulationRadius);

        public VisionSource(VisionMap visionMap, VisionConfig config, IsometricOverlap isometricOverlap, Func<Vector3> position, Func<Owner> owner, Func<float> radius, Func<bool> isAir)
        {
            _position = position;
            _owner = owner;
            _radius = radius;
            _isAir = isAir;
            _visionMap = visionMap;
            _isometricOverlap = isometricOverlap;
            _config = config;
        }

        public bool IsPointVisible(Vector2 point) => _visionResult?.IsPointVisible(point) ?? false;
        
        public async UniTask Recalculate(bool isSimulated)
        {
            if (!isSimulated)
            {
                _visionResult = new VisionResult(Position, new AnimationCurve());
                _visibleUnits = new HashSet<Unit>();
                return;
            }
            
            int areaPoints = _config.UnitVisionPoints;
            AnimationCurve distanceCurve = new();
            
            for (int i = 0; i < areaPoints; i++)
            {
                float rawAngle = 360f / areaPoints * i;
                Vector2 isoDirection = rawAngle.DegreesToVector2() * Isometry.Scale;
                float isoMaxDistance = Radius * isoDirection.magnitude;
                float isoResult;
                
                if (IsAir)
                {
                    isoResult = isoMaxDistance;
                }
                else
                {
                    RaycastHit2D raycast = Physics2D.Raycast(Position, isoDirection, isoMaxDistance, _config.VisionBlockerMask);
                    isoResult = raycast.collider ? raycast.distance : isoMaxDistance;
                }
                
                float rawResult = isoResult / isoDirection.magnitude + _config.AbsoluteExtraSight;
                rawResult = Mathf.Max(rawResult, _config.MinSight);

                Keyframe keyframe = new();
                keyframe.time = rawAngle;
                keyframe.value = rawResult;
                keyframe.weightedMode = WeightedMode.None;

                distanceCurve.AddKey(keyframe);
                if (i == 0)
                {
                    keyframe.time = 360;
                    distanceCurve.AddKey(keyframe);
                }
            }
            _visionResult = new VisionResult(Position, distanceCurve);
            
            _visibleUnits = _isometricOverlap.GetUnits(Position, Radius)
                .Where(u => _visionResult.IsPointVisible(u.Position))
                .Where(u => u.Visibility.IsRevealed || u.Alliance.IsFriendly(Owner))
                .ToHashSet();
        }
    }
}