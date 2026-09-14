using System;
using System.Collections.Generic;
using System.Linq;
using _Core;
using _Core.Pause;
using Gameplay.Data.Configs;
using Save.Data.Units;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units
{
    public class UnitAI : UnitComponent
    {
        protected override string LoadKey => UnitAISaveSystem.LoadKey;

        private readonly IPauseReadonly _tacticalPause;
        private readonly GameTime _gameTime;
        private readonly UnitAiConfig _config;
        private Vector2 _spawnPoint;
        private UnitPatrolPath _patrolPath;

        public Unit DirectThreat { get; private set; }
        public HashSet<Unit> Threats { get; } = new();
        public HashSet<Unit> SurroundingAllies { get; } = new();
        public HashSet<Unit> SurroundingHostiles { get; } = new();
        public HashSet<Unit> SurroundingUnits { get; } = new();
        public Unit PreferredAttackTarget { get; private set; }
        
        public UnitAI(Unit unit, IPauseReadonly tacticalPause, GameTime gameTime, UnitAiConfig config, UnitPatrolPath patrolPath) : base(unit)
        {
            _tacticalPause = tacticalPause;
            _gameTime = gameTime;
            _config = config;
            _patrolPath = patrolPath;
            
            _spawnPoint = unit.Position;

            if (Unit.HasSight)
            {
                Unit.FixedUpdateAsObservable()
                    .Sample(TimeSpan.FromSeconds(_config.TimeBetweenThinking))
                    .Where(_ => _tacticalPause.IsUnpaused)
                    .Subscribe(_ => UpdateSurroundings());
            }
            
            Unit.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.TimeBetweenThinking))
                .Delay(TimeSpan.FromMilliseconds(10))
                .Where(_ => _tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateThreats());
            
            Unit.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.TimeBetweenThinking))
                .Where(_ => _tacticalPause.IsUnpaused)
                .Subscribe(_ => IssueOrder());
        }

        public override IUnitSaveSystem Save()
        {
            return new UnitAISaveSystem(_patrolPath, SerializableVector2.FromVector2(_spawnPoint));
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitAISaveSystem system = GetSaveSystem<UnitAISaveSystem>(saveData);
            _spawnPoint =  system.spawnPoint.ToVector2();
            _patrolPath = system.patrolPath;
        }

        private void IssueOrder()
        {
            if (Unit.Alliance.OwnedByPlayer)
                return;

            if (Threats.Count == 0 && ! UnitType.AIMap.RunWithoutThreats)
            {
                Patrol();
                return;
            }

            HashSet<Unit> targets = SurroundingUnits.Union(Threats).ToHashSet();
            Order order = UnitType.AIMap.GetBestOrder(Unit, targets);
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

            Vector2 destination = _spawnPoint;
            if (_patrolPath != null)
            {
                _patrolPath.TryGetRelativeDestination(_gameTime.Time, out Vector2 relativeDestination);
                destination += relativeDestination;
            }
            Unit.Orders.IssueOrder(new Order(_config.MoveOrder, Unit, OrderTarget.FromPoint(destination)), false);
        }

        private void UpdateSurroundings()
        {
            SurroundingUnits.Clear();
            SurroundingAllies.Clear();
            SurroundingHostiles.Clear();

            foreach (Unit unit in Unit.Sight.VisionSource.VisibleUnits)
            {
                if ( ! unit.Visibility.CanBeTargetedBy(Unit))
                    continue;

                SurroundingUnits.Add(unit);

                if (Unit.Alliance.IsFriendly(unit))
                    SurroundingAllies.Add(unit);

                if (Unit.Alliance.IsHostile(unit))
                    SurroundingHostiles.Add(unit);
            }
        }

        private void UpdateThreats()
        {
            if (Unit.HasLife)
            {
                DirectThreat = _gameTime.Frame - Unit.Life.LastDamageFrame < _config.DamageForgiveTime
                    ? Unit.Life.LastDamageDealer.Alliance.IsHostile(Unit) ? Unit.Life.LastDamageDealer : null
                    : null;
            }

            Threats.Clear();
            foreach (Unit ally in SurroundingAllies)
                foreach (Unit hostile in ally.AI.SurroundingHostiles)
                    Threats.Add(hostile);

            Unit bestTarget = null;
            float bestWorth = 0f;
            foreach (Unit hostile in SurroundingHostiles)
            {
                float worth = _config.AutoAttackEvaluator.EvaluteTargetWorth(Unit, hostile);
                if (bestTarget && worth < bestWorth)
                    continue;
                bestTarget = hostile;
                bestWorth = worth;
            }
            PreferredAttackTarget = bestTarget;
        }
    }
}