using System;
using System.Collections;
using System.Threading.Tasks;
using Extentions.Pause;
using UnityEngine;

namespace Extentions
{
    public class Timer : ITimerWrap
    {
        private readonly MonoBehaviour _container;
        private readonly IPauseRead _pause;
        private int _framesElapsed;
        private bool _active;
        private Coroutine _tickingCoroutine;
        private int _duration;

        public int Duration
        {
            get => _duration;
            set => _duration = Mathf.Max(value, 0);
        }

        public bool Loop { get; set; }

        public int FramesElapsed => _framesElapsed;
        public int FramesLeft => Duration - FramesElapsed;

        public bool IsExpired => FramesLeft <= 0;
        public bool IsOn => _tickingCoroutine != null;
        public bool IsIdle => ! IsOn; 
        
        public WaitForExpire Yield => new WaitForExpire(this);

        public event Action Started;
        public event Action<float> Ticked;
        public event Action Expired;

        public Timer(MonoBehaviour container, int duration = 0, IPauseRead pause = null, bool loop = false)
        {
            _container = container;
            Duration = duration;
            Loop = loop;
            _pause = pause;
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
            _framesElapsed = 0;
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

        private IEnumerator Ticking()
        {
            Started?.Invoke();
            while (_framesElapsed < Duration)
            {
                yield return new WaitForFixedUpdate();
                _framesElapsed++;
                Ticked?.Invoke(FramesLeft);
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
        public int Duration { get; }
        public int FramesElapsed { get; }
        public int FramesLeft { get; }
        
        public bool IsExpired { get; }
        public bool IsOn { get; }
        public bool IsIdle { get; }

        public Timer.WaitForExpire Yield { get; }

        public event Action Started;
        public event Action<float> Ticked;
        public event Action Expired;
    }
}