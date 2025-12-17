using Extentions;
using Extentions.Pause;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    public abstract class StatusType : ScriptableObject
    {
        public abstract void OnAdd(Status status);
        
        public abstract void OnUpdate(Status status);
        
        public abstract void OnRemove(Status status);
    }

    public class Status
    {
        public StatusType Type { get; }
        public Unit Instigator { get; }
        public Unit Host { get; }
        public Timer ExpirationTimer { get; }

        public Status(StatusType type, Unit instigator, Unit host, int duration = 0, IPauseRead pauseRead = null)
        {
            Type = type;
            Instigator = instigator;
            Host = host;
            if (!(duration > 0))
                return;
            ExpirationTimer = new Timer(instigator, duration, pauseRead);
            ExpirationTimer.Start();
            ExpirationTimer.Expired += Remove;
        }

        public void OnAdd() => Type.OnAdd(this);

        public void OnUpdate() => Type.OnUpdate(this);

        public void OnRemove() => Type.OnRemove(this);

        private void Remove()
        {
            Host.Statuses.RemoveStatus(Type);
        }
    }
}