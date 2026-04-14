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
        
        public HashSet<Unit> Threats { get; private set; }
        public HashSet<Unit> SurroundingUnits { get; private set; }
        public Unit PreferredAttackTarget { get; private set; }
        
        public UnitAI(Unit unit, IPauseReadonly tacticalPause, UnitAiConfig config) : base(unit)
        {
            _tacticalPause = tacticalPause;
            _config = config;
            _spawnPoint = unit.Position;
            
            Unit.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.TimeBetweenThinking))
                .Where(_ => _tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateSurroundings());
            
            Unit.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.TimeBetweenThinking))
                .Where(_ => _tacticalPause.IsUnpaused)
                .Subscribe(_ => IssueOrder());
        }

        private void IssueOrder()
        {
            if (Unit.Ownership.OwnedByPlayer)
                return;

            if ( ! Unit.Simulation.IsSimulated || Threats.Count == 0)
            {
                MoveToSpawnPoint();
                return;
            }

            Order order = UnitType.AIMap.GetBestOrder(Unit, SurroundingUnits);
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
            Unit.Orders.IssueOrder(new Order(_config.MoveOrder, Unit, OrderTarget.FromPoint(_spawnPoint)), false);
        }

        private void UpdateSurroundings()
        {
            UpdateSurroundingUnits();
            UpdateThreats(SurroundingUnits);
            UpdatePreferredAttackTarget(Threats);
        }

        private void UpdateSurroundingUnits()
        {
            SurroundingUnits = Unit.Sight.VisibleUnits();
        }

        private void UpdateThreats(HashSet<Unit> surroundingUnits)
        {
            Threats = surroundingUnits.Where(u => u.Ownership.IsHostile(Unit)).ToHashSet();

            Threats.UnionWith(SurroundingUnits
                .Where(u => u.Ownership.IsFriendly(Unit))
                .Where(a => Time.fixedTime - a.Life.LastDamageTime < _config.DamageForgiveTime)
                .Select(a => a.Life.LastDamageDealer)
                .NoNull().ToHashSet());
        }

        private void UpdatePreferredAttackTarget(HashSet<Unit> threats)
        {
            PreferredAttackTarget = threats.MaxElement(t => _config.AutoAttackEvaluator.EvaluteTargetWorth(Unit, t));
        }
    }
}