using System.Collections.Generic;
using Gameplay.Data.Orders;
using Gameplay.Data.Statuses;
using Gameplay.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit Type", order = 0)]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [SerializeField] private int _focusPriority;
        [Space]
        [SerializeField] private UnitTag[] _tags;
        [SerializeField] private int _controlWorth;
        [Space]
        [SerializeField] private int _maxHitPoints;
        [Space]
        [SerializeField] private int _maxShieldPoints;
        [SerializeField] private int _shieldRestoreDelay;
        [SerializeField] private float _shieldPointsPerSecond;
        [Space]
        [SerializeField] private int _maxEnergyPoints;
        [SerializeField] private int _energyRestoreDelay;
        [SerializeField] private float _energyPointsPerSecond;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private bool _isImmobile;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _size;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private bool _isAir;
        [SerializeField] private int _sightRadius;
        [HorizontalLine(2, EColor.White)]
        [Expandable] [SerializeField] private UnitWeaponType _weaponType;
        [SerializeField] private StatusType[] _innateStatuses;
        [SerializeField] private List<OrderType> _availableOrders;
        [HorizontalLine(2, EColor.White)]
        [Expandable] [SerializeField] private UnitSpriteMap _spriteMap;
        [Space]
        [SerializeField] private Vector2 _interactionColliderSize;
        [SerializeField] private Vector2 _interactionColliderOffset;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private UnitAiMap _aiMap;

        public string DisplayName => _displayName;
        public int FocusPriority => _focusPriority;
        public UnitTag[] Tags => _tags;
        public int ControlWorth => _controlWorth;
        public int MaxHitPoints => _maxHitPoints;
        public int MaxShieldPoints => _maxShieldPoints;
        public int ShieldRestoreDelay => _shieldRestoreDelay;
        public float ShieldPointsPerSecond => _shieldPointsPerSecond;
        public int MaxEnergyPoints => _maxEnergyPoints;
        public int EnergyRestoreDelay => _energyRestoreDelay;
        public float EnergyPointsPerSecond => _energyPointsPerSecond;
        public bool IsImmobile => _isImmobile;
        public float MaxSpeed => _maxSpeed;
        public float Size => _size;
        public float RotationSpeed => _rotationSpeed;
        public bool IsAir => _isAir;
        public int SightRadius => _sightRadius;
        public UnitWeaponType WeaponType => _weaponType;
        public StatusType[] InnateStatuses => _innateStatuses;
        public OrderType[] AvailableOrders => _availableOrders.ToArray();
        public UnitSpriteMap SpriteMap => _spriteMap;
        public Vector2 InteractionColliderSize => _interactionColliderSize;
        public Vector2 InteractionColliderOffset => _interactionColliderOffset;
        public UnitAiMap AIMap => _aiMap;

        public PathfindingAgent PathfindingAgent => new(IsAir, Size / 2);

        private void OnValidate()
        {
            _controlWorth = Mathf.Max(_controlWorth, 0);
            _maxHitPoints = Mathf.Max(_maxHitPoints, 1);
            _maxShieldPoints = Mathf.Max(_maxShieldPoints, 0);
            _shieldRestoreDelay = Mathf.Max(_shieldRestoreDelay, 0);
            _shieldPointsPerSecond = Mathf.Max(_shieldPointsPerSecond, 0);
            _maxEnergyPoints  = Mathf.Max(_maxEnergyPoints, 0);
            _energyPointsPerSecond  = Mathf.Max(_energyPointsPerSecond, 0);
            _energyRestoreDelay = Mathf.Max(_energyRestoreDelay, 0);
            _maxSpeed = Mathf.Max(_maxSpeed, 0);
            _size = Mathf.Max(_size, 0.05f);
            _rotationSpeed = Mathf.Max(_rotationSpeed, 0);
            _sightRadius = Mathf.Max(_sightRadius, 0);

            if (IsImmobile)
                _maxSpeed = 0;

            const int availableOrdersCount = 15;
            while (_availableOrders.Count < availableOrdersCount)
            {
                _availableOrders.Add(null);
            }
            while (_availableOrders.Count > availableOrdersCount)
            {
                _availableOrders.RemoveAt(_availableOrders.Count - 1);
            }
        }
    }
}