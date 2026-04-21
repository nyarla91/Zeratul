using System;
using UnityEngine;

namespace Gameplay.Units
{
    [Serializable]
    public struct UnitSpawnInfo
    {
        [SerializeField] private bool _ownedByPlayer;
        [SerializeField] [Range(0, 360)] private float _lookAngle;
        [SerializeField] private UnitPatrolPath _patrolPath;
        
        public bool OwnedByPlayer => _ownedByPlayer;
        public float LookAngle => _lookAngle;
        public UnitPatrolPath PatrolPath => _patrolPath;
    }
}