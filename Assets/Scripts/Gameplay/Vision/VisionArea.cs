using System;
using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionArea : MonoBehaviour
    {
        private readonly List<Unit> _visibleUnits = new();
        
        public void AttachSightArea(Transform sightArea)
        {
            sightArea.SetParent(transform);
            sightArea.gameObject.layer = gameObject.layer;
        }
        
        public bool IsUnitVisible(Unit unit) =>  _visibleUnits.Contains(unit);

        private void OnTriggerEnter2D(Collider2D other)
        {
            Unit unit = other.GetComponentInParent<Unit>();
            if (unit ==  null || _visibleUnits.Contains(unit))
                return;
            _visibleUnits.Add(unit);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Unit unit = other.GetComponentInParent<Unit>();
            if (unit ==  null || ! _visibleUnits.Contains(unit))
                return;
            _visibleUnits.Remove(unit);
        }
    }
}