using System;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitInteractionCollision : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private void Start()
        {
            Vector2 offset = Vector2.up * _unit.Type.Graphics.SpriteHeight;
            transform.localPosition = offset;
            transform.localScale = _unit.Type.Movement.Size * Vector3.one;
        }
    }
}