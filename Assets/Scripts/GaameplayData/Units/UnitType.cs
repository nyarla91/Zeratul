using System;
using UnityEngine;

namespace GaameplayData.Units
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Unit", order = 0)]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private UnitLifeData _life;
        [SerializeField] private UnitMovementData _movement;
        [SerializeField] private UnitWeaponData[] _weapons;

        public UnitLifeData Life => _life;
        public UnitMovementData Movement => _movement;
    }

    [Serializable]
    public struct UnitLifeData
    {
        [SerializeField] private int _maxHitPoints;
        [SerializeField] private int _maxShieldPoints;
        [SerializeField] private float _shieldsRecoveryDelay;

        public int MaxHitPoints => _maxHitPoints;
        public int MaxShieldPoints => _maxShieldPoints;
        public float ShieldsRecoveryDelay => _shieldsRecoveryDelay;
    }

    [Serializable]
    public struct UnitMovementData
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _size;

        public float MaxSpeed => _maxSpeed;
        public float Size => _size;
    }

    [Serializable]
    public struct UnitWeaponData
    {
        [SerializeField] private int _baseDamage;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _range;

        public int BaseDamage => _baseDamage;
        public float Cooldown => _cooldown;
        public float Range => _range;
    }
}