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
    public class UnitAI : UnitComponent
    {
        private readonly IPauseReadonly _tacticalPause;
        private readonly UnitAiConfig _config;
        private readonly Vector2 _spawnPoint;
        
        public UnitAI(Unit unit, IPauseReadonly tacticalPause, UnitAiConfig config) : base(unit)
        {
            _tacticalPause = tacticalPause;
            _config = config;
            _spawnPoint = unit.Position;
            
            Unit.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.TimeBetweenThinking))
                .Where(_ => _tacticalPause.IsUnpaused)
                .Subscribe(_ => IssueOrder());
        }

        private void IssueOrder()
        {
            if (Unit.Ownership.OwnedByPlayer)
                return;

            if ( ! Unit.Simulation.IsSimulated)
            {
                MoveToSpawnPoint();
                return;
            }
            
            HashSet<Unit> surroundings = GetThreats();
            if (surroundings.Count == 0)
            {
                MoveToSpawnPoint();
                return;
            }
            surroundings.UnionWith(Unit.Sight.VisibleUnits());
            Order order = UnitType.AIMap.GetBestOrder(Unit, surroundings);
            if (order == null)
            {
                MoveToSpawnPoint();
                return;
            }
            Unit.Orders.IssueOrder(order, false);
        }

        private void MoveToSpawnPoint()
        {
            if ( ! Unit.CanMove)
                return;
            Unit.Orders.IssueOrder(new Order(_config.MoveOrder, Unit, new OrderTarget(_spawnPoint, null)), false);
        }

        private HashSet<Unit> GetThreats()
        {
            HashSet<Unit> visibleAllies = new() {Unit};
            HashSet<Unit> result = new();

            foreach (Unit visible in Unit.Sight.VisibleUnits())
            {
                if (visible.Ownership.IsHostile(Unit))
                    result.Add(visible);
                else
                    visibleAllies.Add(visible);
            }

            result.UnionWith(visibleAllies
                .Where(a => Time.fixedTime - a.Life.LastDamageTime < _config.DamageForgiveTime)
                .Select(a => a.Life.LastDamageDealer)
                .NoNull().ToHashSet());
            
            return result;
        }
    }
}