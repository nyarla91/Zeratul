using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes
{
    public class Region : MonoBehaviour
    {
        [SerializeField] private PathfindingConfig _config;
        [SerializeField] private Collider2D _collider;

        public event Action<Unit> UnitEntered;
        public event Action<Unit> UnitLeft;

        public HashSet<Unit> UnitsInside
        {
            get
            {
                ContactFilter2D contactFilter = new()
                {
                    useTriggers = true,
                    useLayerMask = true,
                    layerMask = _config.UnitLayerMask
                };

                List<Collider2D> colliders = new();
                _collider.Overlap(contactFilter, colliders);

                return colliders.Select(col => col.GetComponentInParent<Unit>()).ClearNull().ToHashSet();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsUnit(other, out Unit unit))
                UnitEntered?.Invoke(unit);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsUnit(other, out Unit unit))
                UnitLeft?.Invoke(unit);
        }

        private bool IsUnit(Collider2D other, out Unit unit)
        {
            unit = null;
            if (!_config.UnitLayerMask.Includes(other.gameObject.layer))
                return false;
            unit = other.GetComponentInParent<Unit>();
            return true;
        }

        private void OnValidate()
        {
            if (_collider != null)
                _collider.isTrigger = true;
        }
    }
}