using System.Collections.Generic;
using Extentions;
using Gameplay.Vision;
using Zenject;

namespace Gameplay.Units
{
    public class UnitVisibility : UnitComponent
    {
        private readonly List<object> _detectionSources = new();
        
        [Inject] private VisionMap VisionMap { get; set; }
        
        public bool IsVisibleToPlayer => VisionMap.PlayerArea.IsUnitVisible(Composition);
        
        public bool IsVisibleToEnemy => VisionMap.EnemyArea.IsUnitVisible(Composition);
        
        public bool IsVisibleToHostile  => Composition.Ownership.OwnedByPlayer ? IsVisibleToEnemy :  IsVisibleToPlayer;
        
        public bool IsDetected => _detectionSources.Count > 0;
        
        public bool IsCloaked => UnitType.General.IsCloaked && ! IsDetected;
        
        public bool CanBeTargetedBy(Unit targeter) => Composition.Ownership.IsFriendly(targeter) || IsVisibleToHostile;
        
        public void DetectFromSource(object source) => _detectionSources.Add(source);
        
        public void UndetectFromSource(object source) => _detectionSources.TryRemove(source);
    }
}