using System;
using _Core.Pause;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace _Core
{
    public class Timer : ITimerReadonly
    {
        private int _duration;

        public int Duration
        {
            get => _duration;
            set => _duration = Mathf.Max(value, 0);
        }

        public bool Loop { get; set; }

        public int FramesElapsed { get; private set; }
        public int FramesLeft => Duration - FramesElapsed;

        public bool IsExpired => FramesLeft <= 0;
        public bool IsOn { get; private set; }
        public bool IsIdle => ! IsOn; 

        public event Action Started;
        public event Action Expired;

        public Timer(int duration, IPauseReadonly pause = null, bool loop = false)
        {
            Duration = duration;
            Loop = loop;
            
            Observable.EveryFixedUpdate()
                .Where(_ => IsOn)
                .Where(_ => pause == null || pause.IsUnpaused)
                .Subscribe(_ => Tick());
        }

        private void Tick()
        {
            FramesElapsed++;
            
            if (FramesElapsed < Duration)
                return;
            
            Expired?.Invoke();
            
            if (Loop)
                Restart();
            else
                Stop();
        }

        public Timer Start()
        {
            if (Duration == 0)
            {
                Debug.LogWarning("Timer length is 0");
                return this;
            }
            IsOn = true;
            Started?.Invoke();
            return this;
        }

        public Timer Stop()
        {
            IsOn = false;
            return this;
        }

        public Timer Reset()
        {
            Stop();
            FramesElapsed = 0;
            return this;
        }

        public Timer Restart()
        {
            Reset();
            Start();
            return this;
        }

        public async UniTask GetExpirationTask()
        {
            bool expired = false;
            Expired += Expire;
            await UniTask.WaitUntil(() => expired, PlayerLoopTiming.FixedUpdate);
            Expired -= Expire;

            void Expire() => expired = true;
        }
    }

    public interface ITimerReadonly
    {
        int Duration { get; }
        int FramesElapsed { get; }
        int FramesLeft { get; }
        
        bool IsExpired { get; }
        bool IsOn { get; }
        bool IsIdle { get; }

        event Action Started;
        event Action Expired;
        
        UniTask GetExpirationTask();
    }
}