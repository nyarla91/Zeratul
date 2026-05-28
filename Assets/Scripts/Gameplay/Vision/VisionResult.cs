using System;
using Extentions;
using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionResult
    {
        private readonly Vector2 _origin;
        private readonly AnimationCurve _distanceCurve;
        private readonly float[] _maxDistancePerDirection;
        private readonly float _degreesPerIndex;

        public VisionResult(Vector2 origin, AnimationCurve distanceCurve)
        {
            _origin = origin;
            _distanceCurve = distanceCurve;
        }

        public bool IsPointVisible(Vector2 vector)
        {
            if (_distanceCurve.keys.Length == 0)
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
    }
}