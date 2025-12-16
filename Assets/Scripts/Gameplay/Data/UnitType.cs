using System;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Unit", order = 0)]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private int _sightRadius;
        [SerializeField] private UnitLifeData _life;
        [SerializeField] private UnitMovementData _movement;
        [SerializeField] private UnitWeaponType _weapon;
        [SerializeField] private UnitGraphicsData _graphics;
        [SerializeField] private OrderType[] _availableOrders;

        public int SightRadius => _sightRadius;
        public UnitLifeData Life => _life;
        public UnitMovementData Movement => _movement;
        public UnitWeaponType Weapon => _weapon;
        public UnitGraphicsData Graphics => _graphics;
        public OrderType[] AvailableOrders => _availableOrders;
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
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private bool _isAir;

        public float MaxSpeed => _maxSpeed;
        public float Size => _size;
        public float RotationSpeed => _rotationSpeed;
        public bool IsAir => _isAir;
    }

    [Serializable]
    public struct UnitWeaponType
    {
        [SerializeField] private int _baseDamage;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _maxDistance;
        [SerializeField] private bool _autoAttack;

        public int BaseDamage => _baseDamage;
        public float Cooldown => _cooldown;
        public float MaxDistance => _maxDistance;
        public bool AutoAttack => _autoAttack;
    }

    [Serializable]
    public struct UnitGraphicsData
    {
        [SerializeField] private UnitSpriteMap _spriteMap;
        [SerializeField] private float _spriteHeight;
        [SerializeField] private float _shadowSize;
        
        public UnitSpriteMap SpriteMap => _spriteMap;
        public float SpriteHeight => _spriteHeight;
        public float ShadowSize => _shadowSize;
    }
}