using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values.Point
{
    public class ValuePointUnitPosition : SchemeValue<Vector2>
    {
        [SerializeField] private SchemeValue<Unit> _unit;

        public override Vector2 Value => _unit.Value.Position;

        private void OnValidate()
        {
            gameObject.name = $"(Position of {_unit?.gameObject.name})";
        }
    }
}