using System.Collections.Generic;
using _Core;
using Gameplay.Vision;

namespace Gameplay.Units
{
    public class UnitVisibility : UnitComponent
    {
        private readonly VisionMap _visionMap;
        
        private readonly HashSet<object> _detectionSources = new();
        
        private readonly HashSet<object> _cloakSources = new();

        public UnitVisibility(Unit unit, VisionMap visionMap) : base(unit)
        {
            _visionMap = visionMap;
        }

        /// <summary>
        /// Returns true if unit is cloaked by any source
        /// </summary>
        public bool IsCloaked => _cloakSources.Count > 0;
        
        /// <summary>
        /// Returns true if unit is detected by any source
        /// </summary>
        public bool IsDetected => _detectionSources.Count > 0;
        
        /// <summary>
        /// Returns true if unit is cloaked and not detected 
        /// </summary>
        public bool IsHidden => IsCloaked && ! IsDetected;
        
        /// <summary>
        /// Returns true if unit is detected or not cloaked
        /// </summary>
        public bool IsRevealed => ! IsHidden;
        
        /// <summary>
        /// Returns true if unit is visible to targetingUnit's owner
        /// </summary>
        public bool CanBeTargetedBy(Unit other) => CanBeTargetedBy(other.Alliance.CurrentOwner);
        
        public bool CanBeTargetedBy(Owner other) => Unit.IsInteractable && (Unit.Alliance.IsFriendly(other) || IsRevealed) && IsVisibleTo(other);
        
        public void Detect(object source) => _detectionSources.Add(source);
        
        public void StopDetecting(object source) => _detectionSources.Remove(source);
        
        public void Cloak(object source) => _cloakSources.Add(source);
        
        public void Decloak(object source) => _cloakSources.Remove(source);

        public bool IsVisibleTo(Unit other) => IsVisibleTo(other.Alliance.CurrentOwner);
        
        public bool IsVisibleTo(Owner other) => Unit.Alliance.IsFriendly(other) || _visionMap.IsUnitVisibleBy(Unit, other);
    }
}