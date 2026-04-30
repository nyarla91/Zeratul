using System;
using Gameplay.Data.Units;
using UnityEngine;

namespace Gameplay.Units
{
    [Serializable]
    public struct UnitSpawnInfo
    {
        [SerializeField] private UnitType _unitType;
        [SerializeField] private bool _ownedByPlayer;
        [SerializeField] [Range(0, 360)] private float _lookAngle;
        [SerializeField] private UnitPatrolPath _patrolPath;
        
        public UnitType UnitType => _unitType;
        public bool OwnedByPlayer => _ownedByPlayer;
        public float LookAngle => _lookAngle;
        public UnitPatrolPath PatrolPath => _patrolPath;
    }
}