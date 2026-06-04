using _Core.Pause;
using Zenject;

namespace Gameplay
{
    public class TacticalPause : GamePause
    {
        [Inject] private GamePause SystemPause { get; set; }

        public override bool IsPaused => IsPausedSelf || SystemPause.IsPaused;
        
        public bool IsPausedSelf => base.IsPaused;

        public override bool IsUnpaused => ! IsPaused;
    }
}