using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Units
{
    [Serializable]
    public class UnitPatrolPath
    {
        [SerializeField] [JsonProperty] private PatrolWaypoint[] _waypoints;

        private float _totalLoopTime;
        
        public void Init()
        {
            _totalLoopTime = _waypoints.Sum(w => w.ReachTime);
        }

        public bool TryGetRelativeDestination(float time, out Vector2 destination)
        {
            destination = default;
            if (_waypoints.Length == 0)
                return false;
            
            time %= _totalLoopTime;

            int i = 0;
            while (time > 0)
            {
                if (time <= _waypoints[i].ReachTime || i >= _waypoints.Length - 1)
                {
                    destination = _waypoints[i].RelativePoint;
                    Debug.Log(_waypoints[i].RelativePoint);
                    return true;
                }
                time -= _waypoints[i].ReachTime;
                i++;
            }
            return false;
        }

        [Serializable]
        private struct PatrolWaypoint
        {
            [SerializeField] [JsonProperty] private Vector2 _relativePoint;
            [SerializeField] [JsonProperty] private float _reachTime;

            public float ReachTime => _reachTime;
            public Vector2 RelativePoint => _relativePoint;
        }
    }
}
