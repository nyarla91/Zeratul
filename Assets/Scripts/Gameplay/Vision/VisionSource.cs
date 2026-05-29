using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionSource
    {
        private readonly IsometricOverlap _isometricOverlap;
        private readonly VisionConfig _config;

        private VisionResult _result;
        private HashSet<Unit> _visibleUnits = new();
        private Func<Vector3> _position;
        private Func<float> _radius;
        private Func<Owner> _owner;
        private Func<bool> _isAir;

        public HashSet<Unit> VisibleUnits => _visibleUnits
            .Where(u => u.Visibility.IsRevealed || u.Alliance.IsFriendly(Owner))
            .ToHashSet();

        public VisionResult Result => _result;

        public Vector3 Position => _position.Invoke();
        public float Radius => Mathf.Max(_radius.Invoke(), _config.MinSight);
        public Owner Owner => _owner.Invoke();
        public bool IsAir => _isAir.Invoke();
        
        public Bounds Bounds => new(Position, Radius * 2 * Isometry.Scale);
        public Bounds SimulationBounds => new(Position, Radius * 2 * Isometry.Scale + Vector2.one * _config.SimulationRadius);
        
        public bool Disposed { get; private set; }

        public VisionSource(VisionConfig config, IsometricOverlap isometricOverlap, Func<Vector3> position, Func<Owner> owner, Func<float> radius, Func<bool> isAir)
        {
            _position = position;
            _owner = owner;
            _radius = radius;
            _isAir = isAir;
            _isometricOverlap = isometricOverlap;
            _config = config;
        }

        public void Dispose()
        {
            Disposed = true;
            _position = () => Vector3.zero;
            _radius = () => 0;
            _owner = () => Owner.Neutral;
            _isAir = () => false;
        }

        public void Mute()
        {
            _result = default;
            _visibleUnits.Clear();
        }
        
        public void Recalculate()
        {
            AnimationCurve distanceCurve = new();
            bool previousHit = false;
            
            for (float i = 0; i < _config.VisionPoints; i++)
            {
                float rawAngle = 360f / _config.VisionPoints * i;
                bool hit = RaycastInDirection(rawAngle, out Keyframe keyframe);
                distanceCurve.AddKey(keyframe);
                if (i > 0 && hit != previousHit)
                {
                    float step = 1f / (_config.VisionCorrectionPoints + 1);
                    for (float j = i - 1 + step; j < i; j += step)
                    {
                        rawAngle = 360f / _config.VisionPoints * j;
                        RaycastInDirection(rawAngle, out keyframe);
                        distanceCurve.AddKey(keyframe);
                    }
                }
                previousHit = hit;
            }

            Keyframe keyframe360 = new()
            {
                time = 360,
                value = distanceCurve.Evaluate(0),
                weightedMode = WeightedMode.None
            };
            distanceCurve.AddKey(keyframe360);
            
            _result = new VisionResult(Position, distanceCurve);

            _visibleUnits.Clear();
            HashSet<Unit> overlapUnits = _isometricOverlap.GetUnits(Position, Radius);
            foreach (Unit unit in overlapUnits)
            {
                if (unit.Visibility.IsHidden && unit.Alliance.IsHostile(Owner))
                    continue;
                if ( ! _result.IsPointVisible(unit.Position))
                    continue;
                _visibleUnits.Add(unit);
            }
        }

        private bool RaycastInDirection(float rawAngle, out Keyframe keyframe)
        {
            bool result = false;
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
                result = raycast.collider;
                isoResult = result ? raycast.distance : isoMaxDistance;
            }
                
            float rawResult = isoResult / isoDirection.magnitude + _config.AbsoluteExtraSight;
            rawResult = Mathf.Max(rawResult, _config.MinSight);

            keyframe = new Keyframe
            {
                time = rawAngle,
                value = rawResult,
                weightedMode = WeightedMode.None
            };
            return result;
        }
    }
}