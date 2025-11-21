using System;
using System.Collections;
using System.Threading.Tasks;
using Source.Extentions.Pause;
using UnityEngine;

namespace Source.Extentions
{
    public class Timer : ITimerWrap
    {
        private readonly MonoBehaviour _container;
        private readonly IPauseRead _pause;
        private float _timeElapsed;
        private bool _active;
        private Coroutine _tickingCoroutine;
        private float _duration;

        private float DeltaFrame =>
            (_pause == null || _pause.IsUnpaused) ? (FixedTime ? Time.fixedDeltaTime : Time.deltaTime) : 0;

        public float Duration
        {
            get => _duration;
            set => _duration = Mathf.Max(value, 0);
        }

        public bool Loop { get; set; }
        public bool FixedTime { get; set; } = true;

        public float TimeElapsed => _timeElapsed;
        public float TimeLeft => Duration - TimeElapsed;

        public bool IsExpired => TimeLeft <= 0;
        public bool IsOn => _tickingCoroutine != null;
        public bool IsIdle => ! IsOn; 
        
        public WaitForExpire Yield => new WaitForExpire(this);

        public event Action Started;
        public event Action<float> Ticked;
        public event Action Expired;

        public Timer(MonoBehaviour container, float duration = 0, IPauseRead pause = null, bool loop = false, bool fixedTime = true)
        {
            _container = container;
            Duration = duration;
            Loop = loop;
            _pause = pause;
            FixedTime = fixedTime;
            Init();
        }

        public Timer Start()
        {
            if (Duration == 0)
            {
                Debug.LogWarning("Timer length is 0");
                return this;
            }
            if (_tickingCoroutine != null)
                _container.StopCoroutine(_tickingCoroutine);
            _tickingCoroutine = _container.StartCoroutine(Ticking());
            return this;
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Reset()
        {
            Stop();
            _timeElapsed = 0;
        }

        public void Stop()
        {
            if (_tickingCoroutine == null)
                return;
            
            _container.StopCoroutine(_tickingCoroutine);
            _tickingCoroutine = null;
        }

        public async Task GetTask()
        {
            bool expired = false;
            Expired += Expire;
            while (!expired)
                await Task.Delay(20);
            Expired -= Expire;

            void Expire() => expired = true;
        }

        private void Init()
        {
            
        }

        private IEnumerator Ticking()
        {
            Started?.Invoke();
            while (_timeElapsed < Duration)
            {
                if (FixedTime)
                    yield return new WaitForFixedUpdate();
                else
                    yield return null;
                _timeElapsed += DeltaFrame;
                Ticked?.Invoke(TimeLeft);
            }
            
            Expired?.Invoke();
            
            if (Loop)
                Restart();
            else
                Stop();
        }

        public class WaitForExpire : CustomYieldInstruction
        {
            public override bool keepWaiting => ! _expired;

            private bool _expired;
            
            public WaitForExpire(Timer timer)
            {
                timer.Expired += Expire;
            }

            private void Expire() => _expired = true;
        }
    }

    public interface ITimerWrap
    {
        public float Duration { get; }
        public float TimeElapsed { get; }
        public float TimeLeft { get; }
        
        public bool IsExpired { get; }
        public bool IsOn { get; }
        public bool IsIdle { get; }

        public Timer.WaitForExpire Yield { get; }

        public event Action Started;
        public event Action<float> Ticked;
        public event Action Expired;
    }
}