using Extentions.Pause;
using Zenject;

namespace Gameplay
{
    public class TacticalPause : GamePause
    {
        [Inject] private GamePause SystemPause { get; set; }

        public override bool IsPaused => base.IsPaused || SystemPause.IsPaused;

        public override bool IsUnpaused => ! IsPaused;
    }
}