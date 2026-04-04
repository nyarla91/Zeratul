using System;
using System.Collections.Generic;
using System.Linq;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Statuses;
using UniRx;
using Zenject;

namespace Gameplay.Units
{
    public class UnitStatuses : UnitComponent
    {
        private readonly IPauseReadonly _tacticalPause;
        private readonly Dictionary<StatusType, Status> _statuses = new();
        
        public IStatusInfo[] StatusesInfo => _statuses.Values.ToArray<IStatusInfo>();

        public event Action<IStatusInfo> StatusAdded;
        public event Action<IStatusInfo> StatusRemoved;
        
        public UnitStatuses(Unit unit, IPauseReadonly tacticalPause) : base(unit)
        {
            _tacticalPause = tacticalPause;
            foreach (StatusType status in UnitType.InnateStatuses)
            {
                AddStatus(status, Unit);
            }
            
            Observable.EveryFixedUpdate()
                .Where(_ => tacticalPause.IsUnpaused)
                .Subscribe(_ => UpdateStatuses());
        }

        public void AddStatus(StatusType type, Unit instigator, int duration = -1)
        {
            if (_statuses.TryGetValue(type, out Status currentStatus))
            {
                currentStatus.Restart(duration);
                return;
            }
            Status status = new(type,  instigator, Unit, duration, _tacticalPause);
            _statuses.Add(type, status);
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