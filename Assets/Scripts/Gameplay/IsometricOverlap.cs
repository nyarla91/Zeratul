using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay
{
    public class IsometricOverlap : MonoBehaviour
    {
        [SerializeField] private PathfindingConfig _config;
        
        public bool TryGetUnits(Vector2 point, float radius, out HashSet<Unit> units)
        {
            ContactFilter2D contactFilter = new()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = _config.UnitLayerMask
            };

            List<Collider2D> colliders = new();
            Physics2D.OverlapCircle(point, radius, contactFilter, colliders);
            
            units = colliders.Select(col => col.GetComponentInParent<Unit>()).ClearNull().ToHashSet();
            units = units.Where(u => IsUnitInRadius(point, radius, u)).ToHashSet();
            
            return units.Count != 0;
        }

        private bool IsUnitInRadius(Vector2 point, float radius, Unit unit)
        {
            return Isometry.Distance(point, unit) < (radius + unit.Type.Size / 2);
        }
    }
}