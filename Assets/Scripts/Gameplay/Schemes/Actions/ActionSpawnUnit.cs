using System;
using Gameplay.Data.Units;
using Gameplay.Schemes.Values;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionSpawnUnit : SchemeAction
    {
        [SerializeField] private SchemeValue<Vector2> _point;
        [SerializeField] private UnitType _unitType;
        [SerializeField] private UnitSpawnInfo _spawnInfo;
        [SerializeField] private VariableUnit _out;
        
        [Inject] private UnitSpawner UnitSpawner { get; set; }
            
        public override void Act()
        {
            Unit unit = UnitSpawner.Spawn(_point.Value, _unitType, -1, _spawnInfo);
            _out?.Set(unit);
        }

        private void OnValidate()
        {
            gameObject.name = $"Spawn {_unitType.name} at ({_point?.gameObject.name})";
        }
    }
}