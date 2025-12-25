using System;
using System.Collections.Generic;
using System.Linq;
using Extentions.Pause;
using Gameplay.Data;
using Gameplay.Data.Statuses;
using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitStatuses : UnitComponent
    {
        private readonly Dictionary<StatusType, Status> _statuses = new();

        public IStatusInfo[] StatusesInfo => _statuses.Values.ToArray<IStatusInfo>();

        [Inject] private IPauseRead PauseRead { get; set; }

        public void Init(UnitType type)
        {
            foreach (StatusType status in type.InnateStatuses)
            {
                AddStatus(status, Composition);
            }
        }

        public void AddStatus(StatusType type, Unit instigator, int duration = -1)
        {
            if (_statuses.TryGetValue(type, out Status currentStatus))
            {
                currentStatus.Restart(duration);
                return;
            }
            Status status = new(type,  instigator, Composition, duration, PauseRead);
            _statuses.Add(type, status);
            status.OnAdd();
        }

        public void RemoveStatus(StatusType type)
        {
            if ( ! _statuses.TryGetValue(type, out Status status))
                return;
            status.OnRemove();
            _statuses.Remove(type);
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