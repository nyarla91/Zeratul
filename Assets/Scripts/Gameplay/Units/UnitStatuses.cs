using System;
using System.Collections.Generic;
using System.Linq;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Statuses;
using Saving.Data.Units;
using UniRx;
using UniRx.Triggers;

namespace Gameplay.Units
{
    public class UnitStatuses : UnitComponent
    {
        protected override string LoadKey => UnitStatusesSaveData.LoadKey;

        private readonly GameTime _gameTime;
        private readonly GameDataRegistry _gameDataRegistry;
        private readonly IGetUnitByIdService _getUnitByIdService;
        private readonly Dictionary<StatusType, Status> _statuses = new();
        
        public IStatusInfo[] StatusesInfo => _statuses.Values.ToArray<IStatusInfo>();

        public event Action<IStatusInfo> StatusAdded;
        public event Action<IStatusInfo> StatusRemoved;
        
        public UnitStatuses(Unit unit, GameTime gameTime, IPauseReadonly tacticalPause, GameDataRegistry gameDataRegistry,
            IGetUnitByIdService getUnitByIdService) : base(unit)
        {
            _gameTime = gameTime;
            _gameDataRegistry = gameDataRegistry;
            _getUnitByIdService = getUnitByIdService;
            foreach (StatusType status in UnitType.InnateStatuses)
            {
                AddStatus(status, Unit);
            }
            
            Unit.FixedUpdateAsObservable()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateStatuses());
        }

        public override IUnitSaveSystem Save()
        {
            return new UnitStatusesSaveData(_statuses.Values.Select(s => s.ToSaveData()).ToArray());
        }

        public override void ReproduceFromSave(UnitSaveData saveData)
        {
            UnitStatusesSaveData system = GetSaveSystem<UnitStatusesSaveData>(saveData);

            foreach (StatusSaveData statusSaveData in system.statuses)
            {
                AddStatus(statusSaveData);
            }
        }

        public void AddStatus(StatusType type, Unit instigator, int duration = -1)
        {
            AddStatus(new Status(_gameTime, type, instigator, Unit, duration));
        }

        private void AddStatus(StatusSaveData statusSaveData)
        {
            StatusType statusType = _gameDataRegistry.Get<StatusType>(statusSaveData.typeName);
            Unit instigator = _getUnitByIdService.GetUnitById(statusSaveData.instigatorId);
            AddStatus(new Status(_gameTime, statusType, instigator, Unit, statusSaveData.additionFrame, statusSaveData.removalFrame));
        }

        private void AddStatus(Status status)
        {
            if (_statuses.TryGetValue(status.Type, out Status currentStatus))
            {
                currentStatus.Restart(status.RemovalFrame - status.AdditionFrame);
                return;
            }
            _statuses.Add(status.Type, status);
            status.OnAdd();
            StatusAdded?.Invoke(status);
        }

        public void RemoveStatus(StatusType type)
        {
            if ( ! _statuses.TryGetValue(type, out Status status))
                return;
            status.OnRemove();
            _statuses.Remove(type);
            StatusRemoved?.Invoke(status);
        }

        public bool HasStatus(StatusType type) => _statuses.ContainsKey(type);

        private void UpdateStatuses()
        {
            Status[] statuses = _statuses.Values.ToArray();
            for (int i = statuses.Length - 1; i >= 0; i--)
            {
                statuses[i].OnUpdate();
            }
        }
    }
}