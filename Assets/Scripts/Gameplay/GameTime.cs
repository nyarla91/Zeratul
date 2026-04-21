using UniRx;
using Zenject;

namespace Gameplay
{
    public class GameTime
    {
        public float Time { get; private set; }
        public int Frames { get; private set; }
        
        [Inject]
        public GameTime(TacticalPause tacticalPause)
        {
            Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => Tick());
        }

        private void Tick()
        {
            Time += UnityEngine.Time.fixedDeltaTime;
            Frames++;
        }
    }
}