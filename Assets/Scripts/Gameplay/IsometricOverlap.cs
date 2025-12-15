using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class IsometricOverlap : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private LayerMask _unitLayerMask;

        public bool TryGetUnits(Vector2 point, float radius, out Unit[] units)
        {
            transform.position = point;
            transform.localScale = radius * 2 * Vector3.one;
            Physics2D.SyncTransforms();

            ContactFilter2D contactFilter = new()
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = _unitLayerMask
            };

            List<Collider2D> colliders = new();
            _collider.Overlap(contactFilter, colliders);
            if (colliders.Count == 0)
            {
                units = Array.Empty<Unit>();
                return false;
            }
            
            units = colliders.Select(col => col.GetComponentInParent<Unit>()).ClearNull();
            return units.Length != 0;
        }

        private void Update()
        {
            if ( ! Keyboard.current.pKey.wasPressedThisFrame)
                return;
            Vector2 point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            TryGetUnits(point, 5,  out Unit[] units);
            Debug.Log(units.Length);
        }
    }
}