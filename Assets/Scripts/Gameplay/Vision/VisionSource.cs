using System;
using System.Collections.Generic;
using System.Linq;
using _Core;
using Gameplay.Cheats;
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

        public HashSet<Unit> VisibleUnits => _visibleUnits;

        public VisionResult Result => _result;

        public Vector3 Position => _position.Invoke();
        public float Radius => Owner == Owner.Player && CheatMenu.IsEagleVisionToggled ? 30 : Mathf.Max(_radius.Invoke(), _config.MinSight);
        public Owner Owner => _owner.Invoke();
        public bool IsAir => Owner == Owner.Player && CheatMenu.IsEagleVisionToggled || _isAir.Invoke();
        
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
                Keyframe keyframe = RaycastInDirection(rawAngle);
                distanceCurve.AddKey(keyframe);
            }

            Keyframe keyframe360 = new()
            {
                time = 360,
                value = distanceCurve.Evaluate(0),
                weightedMode = WeightedMode.None
            };
            distanceCurve.AddKey(keyframe360);

            for (int i = distanceCurve.keys.Length - 2; i >= 0; i--)
            {
                Keyframe current = distanceCurve.keys[i];
                Keyframe next = distanceCurve.keys[i + 1];
                if (Mathf.Abs(current.value - next.value) < _config.VisionCorrectionTolerance)
                    continue;
                for (float j = 1; j < _config.VisionCorrectionPoints + 1; j++)
                {
                    float angle = Mathf.LerpAngle(current.time, next.time, j / _config.VisionCorrectionTolerance);
                    distanceCurve.AddKey(RaycastInDirection(angle));
                }
            }
            
            _result = new VisionResult(Position, distanceCurve);

            _visibleUnits.Clear();
            HashSet<Unit> overlapUnits = _isometricOverlap.GetUnits(Position, Radius);
            foreach (Unit unit in overlapUnits)
            {
                if ( ! _result.IsPointVisible(unit.Position))
                    continue;
                _visibleUnits.Add(unit);
            }
        }

        private Keyframe RaycastInDirection(float rawAngle)
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

            return new Keyframe
            {
                time = rawAngle,
                value = rawResult,
                weightedMode = WeightedMode.None
            };
        }
    }
}