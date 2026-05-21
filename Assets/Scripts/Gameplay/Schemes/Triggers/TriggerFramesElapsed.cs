using Gameplay.Schemes.Values;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerFramesElapsed : SchemeTrigger
    {
        [SerializeField] private SchemeValue<int> _frame;

        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private GameTime GameTime { get; set; }
        
        private void Awake()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => TacticalPause.IsUnpaused)
                .Where(_ => GameTime.Frame == _frame.Value)
                .Take(1)
                .Subscribe(_ => Trigger());
        }

        private void OnValidate()
        {
            gameObject.name = $"{_frame?.name} frames elapsed";
        }
    }
}