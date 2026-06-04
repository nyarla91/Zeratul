using System;
using _Core;
using UnityEngine;

namespace Gameplay.Vision
{
    public struct VisionResult : IEquatable<VisionResult>
    {
        private readonly Vector2 _origin;
        private readonly AnimationCurve _distanceCurve;
        
        public VisionResult(Vector2 origin, AnimationCurve distanceCurve)
        {
            _origin = origin;
            _distanceCurve = distanceCurve;
        }

        public bool IsPointVisible(Vector2 vector)
        {
            if (_distanceCurve == null)
                return false;
            Vector2 delta = vector - _origin;
            delta /= Isometry.Scale;
            float maxDistance = GetMaxDistanceForDelta(delta);
            return delta.magnitude <= maxDistance;
        }

        private float GetMaxDistanceForDelta(Vector2 delta)
        {
            float angle = delta.ToDegrees();
            while (angle < 0) 
                angle += 360;
            while (angle > 360) 
                angle -= 360;
            return _distanceCurve.Evaluate(angle);
        }

        public bool Equals(VisionResult other)
        {
            return _origin.Equals(other._origin) && Equals(_distanceCurve, other._distanceCurve);
        }

        public override bool Equals(object obj)
        {
            return obj is VisionResult other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_origin, _distanceCurve);
        }
    }
}