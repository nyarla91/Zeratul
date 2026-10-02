using System;
using System.Collections.Generic;
using _Core;
using UnityEngine;

namespace Gameplay.Vision
{
    public struct VisionResult : IEquatable<VisionResult>
    {
        private const int DistancesCapacity = 90; 
        private readonly Vector2 _origin;
        private readonly float[] _distances;

        public Vector2[] Points
        {
            get
            {
                Vector2[] result = new Vector2[_distances.Length];
                for (int i = 0; i < _distances.Length; i++)
                {
                    float distance = _distances[i];
                    result[i] = _origin + distance.DegreesToVector2() * Isometry.Scale;
                }
                return result;
            }
        }
        
        public VisionResult(Vector2 origin, AnimationCurve distanceCurve)
        {
            _origin = origin;
            _distances = new float[DistancesCapacity];
            for (int i = 0; i < DistancesCapacity; i++)
            {
                _distances[i] = distanceCurve.Evaluate((float) i / DistancesCapacity * 360f);
            }
        }

        public bool IsMuted() => _distances == null;

        public bool IsPointVisible(Vector2 vector)
        {
            if (_distances == null)
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
            while (angle >= 360) 
                angle -= 360;
            return _distances[Mathf.FloorToInt(angle / 360 * DistancesCapacity)];
        }

        public bool Equals(VisionResult other)
        {
            return _origin.Equals(other._origin) && Equals(_distances, other._distances);
        }

        public override bool Equals(object obj)
        {
            return obj is VisionResult other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_origin, _distances);
        }
    }
}