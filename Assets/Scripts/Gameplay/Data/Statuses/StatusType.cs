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
        public int AdditionFrame { get; private set; }
        public int RemovalFrame { get; private set; }
        public int CurrentFrame { get; private set; }

        public int FramesLeft => RemovalFrame - CurrentFrame;

        public Status(StatusType type, Unit instigator, Unit host, int duration = -1, IPauseRead pauseRead = null)
        {
            Type = type;
            Instigator = instigator;
            Host = host;
            AdditionFrame = 0;
            RemovalFrame = duration;
        }

        public void OnAdd() => Type.OnAdd(this);

        public void OnUpdate()
        {
            Type.OnUpdate(this);
            CurrentFrame++;
            if (CurrentFrame == RemovalFrame)
                Remove();
        }

        public void OnRemove() => Type.OnRemove(this);

        public void Restart(int newDuration)
        {
            if (RemovalFrame == -1  || FramesLeft > newDuration)
                return;
            AdditionFrame = CurrentFrame;
            RemovalFrame = CurrentFrame + newDuration;
        }

        private void Remove()
        {
            Host.Statuses.RemoveStatus(Type);
        }
    }
}