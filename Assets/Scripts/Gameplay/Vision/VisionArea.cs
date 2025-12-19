using System;
using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Vision
{
    public class VisionArea : MonoBehaviour
    {
        private readonly List<Unit> _visibleUnits = new();
        private bool _isInitialized;
        
        public bool IsOwnedByPlayer { get; private set; }
        
        public void Init(bool isOwnedByPlayer)
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            IsOwnedByPlayer = isOwnedByPlayer;
        }
        
        public void AttachSightArea(Transform sightArea)
        {
            sightArea.SetParent(transform);
            sightArea.gameObject.layer = gameObject.layer;
        }
        
        public bool IsUnitVisible(Unit unit)
            => unit.Ownership.OwnedByPlayer == IsOwnedByPlayer 
                || (_visibleUnits.Contains(unit) && ! unit.Visibility.IsCloaked);

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