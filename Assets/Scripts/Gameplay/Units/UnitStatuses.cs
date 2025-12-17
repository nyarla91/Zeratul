using System;
using System.Collections.Generic;
using Extentions.Pause;
using Gameplay.Data.Statuses;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitStatuses : UnitComponent
    {
        private Dictionary<StatusType, Status> _statuses = new();

        [Inject] private IPauseRead PauseRead { get; set; }
        
        public void AddStatus(StatusType type, Unit instigator, int duration = 0)
        {
            if (_statuses.TryGetValue(type, out Status currentStatus))
            {
                if (duration < currentStatus.ExpirationTimer.FramesLeft)
                    return;
                currentStatus.ExpirationTimer.Duration = duration;
                currentStatus.ExpirationTimer.Restart();
                return;
            }
            Status status = new(type,  instigator, Composition, duration, PauseRead);
            _statuses.Add(type, status);
            status.OnAdd();
        }

        public void RemoveStatus(StatusType type)
        {
            if ( ! _statuses.ContainsKey(type))
                return;
            _statuses[type].OnRemove();
            _statuses.Remove(type);
        }

        private void FixedUpdate()
        {
            foreach (Status status in _statuses.Values)
            {
                status.OnUpdate();
            }
        }
    }
}