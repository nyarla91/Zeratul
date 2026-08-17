using System.Collections.Generic;
using Gameplay.Data.AiEvaluators;
using Gameplay.Data.Orders;
using Gameplay.Data.Statuses;
using Gameplay.Map;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit Type", order = 0)]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [SerializeField] private int _focusPriority;
        [SerializeField] private bool _nonInteractable;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private UnitTag[] _tags;
        [SerializeField] private int _controlCost;
        [SerializeField] private int _controlSlots;
        [SerializeField] private AiUnitTargetEvaluatorGroup _attackWorth;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private bool _isInvulnerable;
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
        [SerializeField] private bool _disableCollision;
        [SerializeField] private bool _isImmobile;
        [SerializeField] private bool _isAir;
        [Space]
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _size;
        [SerializeField] private float _rotationSpeed;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private bool _disableSight;
        [SerializeField] private int _sightRadius;
        [HorizontalLine(2, EColor.White)]
        [Expandable] [SerializeField] private UnitWeaponType _weaponType;
        [SerializeField] private StatusType[] _innateStatuses;
        [SerializeField] private List<OrderType> _availableOrders;
        [HorizontalLine(2, EColor.White)]
        [Expandable] [SerializeField] private UnitSpriteMap _spriteMap;
        [Space]
        [SerializeField] private bool _hideOnMinimap;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private UnitAiMap _aiMap;
        [HorizontalLine(2, EColor.White)]
        [SerializeField] private ReferenceIRadiusSource _editorRadius;
        [SerializeField] private Color _editorRadiusColor;

        public string DisplayName => _displayName;
        public int FocusPriority => _focusPriority;
        public bool NonInteractable => _nonInteractable;
        public UnitTag[] Tags => _tags;
        public int ControlCost => _controlCost;
        public int ControlSlots => _controlSlots;
        public AiUnitTargetEvaluatorGroup AttackWorth => _attackWorth;
        public bool IsInvulnerable => _isInvulnerable;
        public int MaxHitPoints => _maxHitPoints;
        public int MaxShieldPoints => _maxShieldPoints;
        public int ShieldRestoreDelay => _shieldRestoreDelay;
        public float ShieldPointsPerSecond => _shieldPointsPerSecond;
        public int MaxEnergyPoints => _maxEnergyPoints;
        public int EnergyRestoreDelay => _energyRestoreDelay;
        public float EnergyPointsPerSecond => _energyPointsPerSecond;
        public bool DisableCollision => _disableCollision;
        public bool IsImmobile => _isImmobile;
        public float MaxSpeed => _maxSpeed;
        public float Size => _size;
        public float RotationSpeed => _rotationSpeed;
        public bool IsAir => _isAir;
        public bool DisableSight => _disableSight;
        public int SightRadius => _sightRadius;
        public UnitWeaponType WeaponType => _weaponType;
        public StatusType[] InnateStatuses => _innateStatuses;
        public OrderType[] AvailableOrders => _availableOrders.ToArray();
        public UnitSpriteMap SpriteMap => _spriteMap;
        public bool HideOnMinimap => _hideOnMinimap;
        public UnitAiMap AIMap => _aiMap;
        public float EditorRadius => _editorRadius?.I?.Radius ?? 0;
        public Color EditorRadiusColor => _editorRadiusColor;

        public PathfindingAgent PathfindingAgent => new(IsAir, Size / 2);

        private void OnValidate()
        {
            _controlCost = Mathf.Max(_controlCost, 0);
            _controlSlots = Mathf.Max(_controlSlots, 0);
            _maxHitPoints = IsInvulnerable ? 0 : Mathf.Max(_maxHitPoints, 1);
            _maxShieldPoints = IsInvulnerable ? 0 : Mathf.Max(_maxShieldPoints, 0);
            _shieldRestoreDelay = Mathf.Max(_shieldRestoreDelay, 0);
            _shieldPointsPerSecond = Mathf.Max(_shieldPointsPerSecond, 0);
            _maxEnergyPoints  = Mathf.Max(_maxEnergyPoints, 0);
            _energyPointsPerSecond  = Mathf.Max(_energyPointsPerSecond, 0);
            _energyRestoreDelay = Mathf.Max(_energyRestoreDelay, 0);
            _maxSpeed = Mathf.Max(_maxSpeed, 0);
            _size = Mathf.Max(_size, 0.05f);
            _rotationSpeed = Mathf.Max(_rotationSpeed, 0);
            _sightRadius = DisableSight ? 0 : Mathf.Max(_sightRadius, 0);

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