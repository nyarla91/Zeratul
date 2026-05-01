using Saving.Data;
using UniRx;
using Zenject;

namespace Gameplay
{
    public class GameTime
    {
        private readonly TacticalPause _tacticalPause;
        
        public int UnpausedFrame { get; private set; }
        public int Frame { get; private set; }
        public float UnpausedTime => UnityEngine.Time.fixedDeltaTime * UnpausedFrame;
        public float Time => UnityEngine.Time.fixedDeltaTime * Frame;
        
        [Inject]
        public GameTime(TacticalPause tacticalPause)
        {
            _tacticalPause = tacticalPause;
            Observable.EveryFixedUpdate()
                .Subscribe(_ => Tick());
        }

        public void ReproduceFromSaveData(GeneralSaveSystem saveSystem)
        {
            Frame = saveSystem.gameTimeFrame;
            UnpausedFrame = saveSystem.gameTimeUnpausedFrame;
        }

        private void Tick()
        {
            UnpausedFrame++;
            if (_tacticalPause.IsUnpaused)
                Frame++;
        }
    }
}