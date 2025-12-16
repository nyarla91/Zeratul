using Gameplay.Vision;
using Zenject;

namespace Gameplay.Units
{
    public class UnitVisibility : UnitComponent 
    {
        [Inject] private VisionMap VisionMap { get; set; }

        public bool IsVisibleToPlayer => VisionMap.PlayerArea.IsUnitVisible(Composition);
        
        public bool IsVisibleToEnemy => VisionMap.EnemyArea.IsUnitVisible(Composition);
        
        public bool IsVisibleToHostile  => Composition.Ownership.OwnedByPlayer ? IsVisibleToEnemy :  IsVisibleToPlayer;
    }
}