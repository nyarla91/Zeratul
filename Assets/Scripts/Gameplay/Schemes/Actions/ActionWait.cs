using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Actions
{
    public class ActionWait : SchemeAction
    {
        [SerializeField] private int _frames;
        [SerializeField] private bool _useUnpaused;
        
        private int CurrentFrame => _useUnpaused ? GameTime.UnpausedFrame : GameTime.Frame;
        
        [Inject] private GameTime GameTime { get; set; }
        
        public override async UniTask Act()
        {
            int targetFrame = CurrentFrame + _frames;
            await UniTask.WaitUntil(() => CurrentFrame == targetFrame, PlayerLoopTiming.LastFixedUpdate);
        }

        private void OnValidate()
        {
            string frameName = _useUnpaused ? "unpaused frames" : "frames";
            gameObject.name = $"> ... Wait for {_frames} {frameName} ...";
        }
    }
}