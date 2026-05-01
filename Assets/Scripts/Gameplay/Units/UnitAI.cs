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
        private readonly GameTime _gameTime;
        private readonly UnitAiConfig _config;
        private readonly UnitPatrolPath _patrolPath;
        private readonly Vector2 _spawnPoint;

        public HashSet<Unit> Threats { get; private set; }
        public HashSet<Unit> SurroundingUnits { get; private set; }
        public Unit PreferredAttackTarget { get; private set; }
        
        public UnitAI(Unit unit, IPauseReadonly tacticalPause, GameTime gameTime, UnitAiConfig config, UnitPatrolPath patrolPath) : base(unit)
        {
            _tacticalPause = tacticalPause;
            _gameTime = gameTime;
            _config = config;
            _patrolPath = patrolPath;
            
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
            if (Unit.Alliance.OwnedByPlayer)
                return;

            if ( ! Unit.Simulation.IsSimulated || Threats.Count == 0)
            {
                Patrol();
                return;
            }

            Order order = UnitType.AIMap.GetBestOrder(Unit, SurroundingUnits);
            if (order == null)
            {
                Patrol();
                return;
            }
            Unit.Orders.IssueOrder(order, false);
        }

        private void Patrol()
        {
            if ( ! Unit.CanMove)
                return;
            _patrolPath.TryGetRelativeDestination(_gameTime.Time, out Vector2 destination);
            destination += _spawnPoint;
            Unit.Orders.IssueOrder(new Order(_config.MoveOrder, Unit, OrderTarget.FromPoint(destination)), false);
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
            Threats = surroundingUnits.Where(u => u.Alliance.IsHostile(Unit)).ToHashSet();

            Threats.UnionWith(surroundingUnits
                .Where(u => u.Alliance.IsFriendly(Unit))
                .Where(a => Time.fixedTime - a.Life.LastDamageTime < _config.DamageForgiveTime)
                .Select(a => a.Life.LastDamageDealer)
                .Where(u => u.Alliance.IsHostile(Unit))
                .NoNull().ToHashSet());
        }

        private void UpdatePreferredAttackTarget(HashSet<Unit> threats)
        {
            PreferredAttackTarget = threats.MaxElement(t => _config.AutoAttackEvaluator.EvaluteTargetWorth(Unit, t));
        }
    }
}