using Cysharp.Threading.Tasks;
using Zenject;

namespace Gameplay.Units
{
    public class UnitStagger : UnitComponent
    {
        public int RecoveryFramesLeft { get; private set; }
        public bool IsStaggered { get; private set; }
        public string Action { get; private set; }
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        
        public async UniTask<bool> TryBegin(int windupTime, int recoveryTime, string action)
        {
            if (IsStaggered)
            {
                return false;
            }
            IsStaggered = true;
            Action = action;

            for (int i = 0; i < windupTime; i++)
            {
                if (TacticalPause.IsPaused)
                    await UniTask.WaitUntil(() => TacticalPause.IsUnpaused, PlayerLoopTiming.FixedUpdate);
                await UniTask.WaitForFixedUpdate();
            }
            RecoveryFramesLeft = recoveryTime;
            return true;
        }

        private void FixedUpdate()
        {
            if (TacticalPause.IsPaused)
                return;
            
            if (RecoveryFramesLeft == 0)
            {
                IsStaggered = false;
            }
            RecoveryFramesLeft--;
        }
    }
}