using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Triggers
{
    public class TriggerPeriodicEvent : SchemeTrigger
    {
        [SerializeField] private int _framePeriod;

        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private GameTime GameTime { get; set; }
        
        private void Awake()
        {
            Observable.EveryFixedUpdate()
                .Where(_ => TacticalPause.IsUnpaused)
                .Where(_ => GameTime.Frame % _framePeriod == 0)
                .Subscribe(_ => Trigger());
        }

        private void OnValidate()
        {
            _framePeriod = Mathf.Max(_framePeriod, 1);
            gameObject.name = $"Every {_framePeriod} frames";
        }
    }
}