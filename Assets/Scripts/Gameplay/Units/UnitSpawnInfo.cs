using System;
using Extentions;
using UnityEngine;

namespace Gameplay.Units
{
    [Serializable]
    public class UnitSpawnInfo
    {
        [SerializeField] private Owner _owner;
        [SerializeField] [Range(0, 360)] private float _lookAngle;
        [SerializeField] private UnitPatrolPath _patrolPath;

        public Owner Owner => _owner;
        public float LookAngle => _lookAngle;
        public UnitPatrolPath PatrolPath => _patrolPath;

        public UnitSpawnInfo(Owner owner, float lookAngle, UnitPatrolPath patrolPath)
        {
            _owner = owner;
            _lookAngle = lookAngle;
            _patrolPath = patrolPath;
        }
    }
}