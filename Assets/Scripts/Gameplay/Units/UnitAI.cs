using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Extentions.Pause;
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

        public HashSet<Unit> DirectThreats { get; private set; }
        public HashSet<Unit> Threats { get; private set; }
        public HashSet<Unit> SurroundingAllies { get; private set; }
        public HashSet<Unit> SurroundingHostiles { get; private set; }
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

            if (Threats.Count == 0)
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
            SurroundingUnits = Unit.Sight.VisibleUnits();

            SurroundingAllies = SurroundingUnits
                .Where(u => Unit.Alliance.IsFriendly(u))
                .ToHashSet();

            SurroundingHostiles = SurroundingUnits
                .Where(u => Unit.Alliance.IsHostile(u))
                .ToHashSet();
            
            DirectThreats = SurroundingAllies
                .Where(a => Time.fixedTime - a.Life.LastDamageFrame < _config.DamageForgiveTime)
                .Select(a => a.Life.LastDamageDealer)
                .Where(u => u?.Alliance.IsHostile(Unit) ?? false)
                .ClearNull()
                .ToHashSet();

            Threats = SurroundingHostiles
                .Union(DirectThreats)
                .ToHashSet();
            
            PreferredAttackTarget = DirectThreats.MaxElement(t => _config.AutoAttackEvaluator.EvaluteTargetWorth(Unit, t));
        }
    }
}