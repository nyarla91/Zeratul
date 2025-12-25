using System.Collections.Generic;
using Extentions;
using Gameplay.Vision;
using Zenject;

namespace Gameplay.Units
{
    public class UnitVisibility : UnitComponent
    {
        private readonly List<object> _detectionSources = new();
        
        private readonly List<object> _cloakSources = new();
        
        [Inject] private VisionMap VisionMap { get; set; }

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
        /// Returns true if unit is revealed and is in sight of any player's unit or owned by player
        /// </summary>
        public bool IsVisibleToPlayer => VisionMap.PlayerArea.IsUnitVisible(Composition);

        /// <summary>
        /// Returns true if unit is revealed and is in sight of any enemy unit or owned by enemy
        /// </summary>
        public bool IsVisibleToEnemy => VisionMap.EnemyArea.IsUnitVisible(Composition);

        /// <summary>
        /// Returns true if unit is revealed and is in sight of any unit owned by hostile
        /// </summary>
        public bool IsVisibleToHostile  => Composition.Ownership.OwnedByPlayer ? IsVisibleToEnemy :  IsVisibleToPlayer;
        
        /// <summary>
        /// Returns true if unit is visible to targetingUnit's owner
        /// </summary>
        public bool CanBeTargetedBy(Unit targetingUnit) => Composition.Ownership.IsFriendly(targetingUnit) || IsVisibleToHostile;
        
        public void Detect(object source) => _detectionSources.Add(source);
        
        public void StopDetecting(object source) => _detectionSources.Remove(source);
        
        public void Cloak(object source) => _cloakSources.Add(source);
        
        public void Decloak(object source) => _cloakSources.Remove(source);
    }
}