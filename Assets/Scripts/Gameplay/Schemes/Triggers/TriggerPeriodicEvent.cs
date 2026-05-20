using System;
using Gameplay.Schemes.Values;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerPeriodicEvent : SchemeTrigger
    {
        [SerializeField] private SchemeValue<int> _framePeriod;

        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private GameTime GameTime { get; set; }
        
        private void Awake()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => TacticalPause.IsUnpaused)
                .Where(_ => GameTime.Frame % _framePeriod.Value == 0)
                .Subscribe(_ => Trigger());
        }

        private void OnValidate()
        {
            gameObject.name = $"Every {_framePeriod?.name} frames";
        }
    }
}