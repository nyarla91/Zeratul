using System;
using UnityEngine;

namespace GaameplayData.Units
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Unit", order = 0)]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private UnitMovementData _movement;

        public UnitMovementData Movement => _movement;
    }

    [Serializable]
    public struct UnitMovementData
    {
        [SerializeField] private float _maxSpeed;

        public float MaxSpeed => _maxSpeed;
    }
}