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

        public HashSet<Unit> GetUnits(Vector2 point, float radius)
        {
            ContactFilter2D contactFilter = new()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = _config.UnitLayerMask
            };

            List<Collider2D> colliders = new();
            Physics2D.OverlapCircle(point, radius, contactFilter, colliders);
            
            HashSet<Unit> result = new();
            foreach (Collider2D collider in colliders)
            {
                Unit unit = collider.GetComponentInParent<Unit>();
                if (unit && IsUnitInRadius(point, radius, unit))
                    result.Add(unit);
            }
            return result;
        }
        
        public bool TryGetUnits(Vector2 point, float radius, out HashSet<Unit> units)
        {
            units = GetUnits(point, radius);
            return units.Count != 0;
        }

        private bool IsUnitInRadius(Vector2 point, float radius, Unit unit)
        {
            return Isometry.Distance(point, unit) < radius;
        }
    }
}