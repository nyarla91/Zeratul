using System;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Unit Movement Config", order = 0)]
    public class UnitMovementConfig : ScriptableObject
    {
        [SerializeField]  private float _nodeProximityDistance;
        [SerializeField]  private float _minPathRecalculationPeriod;
        [SerializeField]  [Range(0, 1)] private float _avoidanceAdditionalRadius;
        [SerializeField]  [Range(0, 1)] private float _avoidanceStrength;

        public float NodeProximityDistance => _nodeProximityDistance;
        public float MinPathRecalculationPeriod => _minPathRecalculationPeriod;
        public float AvoidanceAdditionalRadius => _avoidanceAdditionalRadius;
        public float AvoidanceStrength => _avoidanceStrength;
    }
}