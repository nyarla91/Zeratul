using Cysharp.Threading.Tasks;

namespace Gameplay.Units
{
    public class UnitStagger : UnitComponent
    {
        private int _recoveryFramesLeft;
        
        public bool IsStaggered { get; private set; }
        public string Action { get; private set; }
        
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
                await UniTask.WaitForFixedUpdate();
            }
            _recoveryFramesLeft = recoveryTime;
            return true;
        }

        private void FixedUpdate()
        {
            if (_recoveryFramesLeft == 0)
            {
                IsStaggered = false;
                Action = "";
            }
            _recoveryFramesLeft--;
        }
    }
}