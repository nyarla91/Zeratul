using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class UnitPatrolPath
    {
        [SerializeField] [JsonProperty] private PatrolWaypoint[] _waypoints;

        [JsonIgnore] public PatrolWaypoint[] Waypoints => _waypoints;
        [JsonIgnore] public float TotalLoopTime => _waypoints.Sum(w => w.ReachTime);

        public bool TryGetRelativeDestination(float time, out Vector2 destination)
        {
            destination = default;
            if (_waypoints.Length == 0)
                return false;
            
            time %= TotalLoopTime;

            int i = 0;
            while (time > 0)
            {
                if (time <= _waypoints[i].ReachTime || i >= _waypoints.Length - 1)
                {
                    destination = _waypoints[i].RelativePoint;
                    return true;
                }
                time -= _waypoints[i].ReachTime;
                i++;
            }
            return false;
        }

    }
    
    [Serializable]
    public struct PatrolWaypoint
    {
        [SerializeField] [JsonProperty] private SerializableVector2 _relativePoint;
        [SerializeField] [JsonProperty] private float _reachTime;

        [JsonIgnore] public float ReachTime => _reachTime;
        [JsonIgnore] public Vector2 RelativePoint => _relativePoint.ToVector2();
    }
}
