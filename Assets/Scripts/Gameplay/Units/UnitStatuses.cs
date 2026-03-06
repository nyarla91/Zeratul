using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Gameplay.Data.Statuses;
using Zenject;

namespace Gameplay.Units
{
    public class UnitStatuses : UnitComponentMono
    {
        private readonly Dictionary<StatusType, Status> _statuses = new();

        public IStatusInfo[] StatusesInfo => _statuses.Values.ToArray<IStatusInfo>();

        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        public void Init(UnitType type)
        {
            foreach (StatusType status in type.InnateStatuses)
            {
                AddStatus(status, Unit);
            }
        }

        public void AddStatus(StatusType type, Unit instigator, int duration = -1)
        {
            if (_statuses.TryGetValue(type, out Status currentStatus))
            {
                currentStatus.Restart(duration);
                return;
            }
            Status status = new(type,  instigator, Unit, duration, TacticalPause);
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

        private void FixedUpdate()
        {
            Status[] statuses = _statuses.Values.ToArray();
            for (int i = statuses.Length - 1; i >= 0; i--)
            {
                statuses[i].OnUpdate();
            }
        }
    }
}