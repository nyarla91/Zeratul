using System;
using _Core.Pause;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

namespace Gameplay.Units
{
    public class UnitStagger : UnitComponent
    {
        private readonly IPauseReadonly _tacticalPause;
        
        public int RecoveryFramesLeft { get; private set; }
        public bool IsStaggered { get; private set; }
        public bool IsRecovering { get; private set; }

        public string Action { get; private set; } = "idle";

        public event Action Began;
        
        public UnitStagger(Unit unit, IPauseReadonly tacticalPause) : base(unit)
        {
            _tacticalPause = tacticalPause;

            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateRecovery());
        }

        public async UniTask<bool> TryBegin(int windupTime, int recoveryTime, string action)
        {
            if (IsStaggered || IsRecovering)
            {
                return false;
            }
            IsStaggered = true;
            IsRecovering = false;
            Action = action;
            Began?.Invoke();

            for (int i = 0; i < windupTime; i++)
            {
                if (_tacticalPause.IsPaused)
                    await UniTask.WaitUntil(() => _tacticalPause.IsUnpaused, PlayerLoopTiming.FixedUpdate);
                await UniTask.WaitForFixedUpdate();
            }
            RecoveryFramesLeft = recoveryTime;
            IsStaggered = false;
            IsRecovering = true;
            return true;
        }

        private void UpdateRecovery()
        {
            if (RecoveryFramesLeft == 0)
            {
                IsRecovering = false;
            }
            RecoveryFramesLeft--;
        }
    }
}