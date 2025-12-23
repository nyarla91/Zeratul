using Gameplay.Data.Orders;
using Gameplay.Data.Statuses;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit", order = 0)]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private int _maxHitPoints;
        [Space]
        [SerializeField] private int _maxShieldPoints;
        [SerializeField] private int _shieldsRecoveryDelay;
        [Space]
        [SerializeField] private int _maxEnergyPoints;
        [SerializeField] private int _energyRestorePeriod = 50;
        [ReadOnly] [SerializeField] private float _energyPerSecond;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _size;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private bool _isAir;
        [SerializeField] private int _sightRadius;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private bool _isCloaked;
        [Expandable] [SerializeField] private UnitWeaponType _weaponType;
        [SerializeField] private StatusType[] _innateStatuses;
        [SerializeField] private OrderType[] _availableOrders;
        [HorizontalLine(2, EColor.White)]
        [Expandable] [SerializeField] private UnitSpriteMap _spriteMap;

        public int MaxHitPoints => _maxHitPoints;
        public int MaxShieldPoints => _maxShieldPoints;
        public int ShieldsRecoveryDelay => _shieldsRecoveryDelay;
        public int MaxEnergyPoints => _maxEnergyPoints;
        public int EnergyRestorePeriod => _energyRestorePeriod;
        public float MaxSpeed => _maxSpeed;
        public float Size => _size;
        public float RotationSpeed => _rotationSpeed;
        public bool IsAir => _isAir;
        public int SightRadius => _sightRadius;
        public bool IsCloaked => _isCloaked;
        public UnitWeaponType WeaponType => _weaponType;
        public StatusType[] InnateStatuses => _innateStatuses;
        public OrderType[] AvailableOrders => _availableOrders;
        public UnitSpriteMap SpriteMap => _spriteMap;

        private void OnValidate()
        {
            _maxHitPoints = Mathf.Max(_maxHitPoints, 1);
            _maxShieldPoints = Mathf.Max(_maxShieldPoints, 0);
            _shieldsRecoveryDelay = Mathf.Max(_shieldsRecoveryDelay, 0);
            _maxEnergyPoints  = Mathf.Max(_maxEnergyPoints, 0);
            _energyRestorePeriod = Mathf.Max(_energyRestorePeriod, 1);
            _energyPerSecond = 1 / (Time.fixedDeltaTime * _energyRestorePeriod);
            _maxSpeed = Mathf.Max(_maxSpeed, 0);
            _size = Mathf.Max(_size, 0.05f);
            _rotationSpeed = Mathf.Max(_rotationSpeed, 0);
            _sightRadius = Mathf.Max(_sightRadius, 0);
        }
    }
}