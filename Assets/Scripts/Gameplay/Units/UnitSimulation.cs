using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Extentions.Pause;
using Gameplay.Data.Configs;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitSimulation : UnitComponent
    {
        private Collider2D _simulationCollider;
        private readonly VisionConfig _config;

        public bool IsSimulated { get; private set; }

        public UnitSimulation(Unit unit, IPauseReadonly tacticalPause, VisionConfig config, Collider2D simulationCollider) : base(unit)
        {
            _config = config;
            _simulationCollider = simulationCollider;

            _simulationCollider.transform.localScale = config.SimulationRadius * 2 * Vector3.one;

            Unit.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.RecalculationPeriod))
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateSimulated());

        }

        private void UpdateSimulated()
        {
            bool simulationSource = Unit.Alliance.IsFriendly(Owner.Player);
            _simulationCollider.enabled = simulationSource;
            if (simulationSource)
            {
                IsSimulated = true;
                return;
            }
            
            ContactFilter2D contactFilter = new();
            contactFilter.useLayerMask = true;
            contactFilter.useTriggers = true;
            contactFilter.layerMask = _config.SimulationMask;

            IsSimulated = Physics2D.OverlapPoint(Unit.Position, contactFilter, new Collider2D[1]) > 0;
        }
    }
}