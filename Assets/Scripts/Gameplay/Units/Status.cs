using Gameplay.Data.Statuses;
using Gameplay.UI;
using Saving.Data.Units;
using UnityEngine;

namespace Gameplay.Units
{
    public class Status : IStatusInfo
    {
        private readonly GameTime _gameTime;
        
        public StatusType Type { get; }
        public Unit Instigator { get; }
        public Unit Host { get; }
        public int AdditionFrame { get; private set; }
        public int RemovalFrame { get; private set; }

        public int FramesSinceAddition => _gameTime.Frame - AdditionFrame;
        public bool IsLocked => Type.IsLocked(this);

        public int FramesLeft => RemovalFrame - _gameTime.Frame;
        
        public string DisplayDescription
        {
            get
            {
                string result = Type.RawDisplayDescription;
                if (FramesLeft < 3)
                    return result;
                int secondsLeft = Mathf.CeilToInt(Time.fixedDeltaTime * FramesLeft);
                result += $"<stat>\n{secondsLeft} sec. left</stat>";
                return result;
            }
        }

        public TooltipInfo TooltipInfo => Type.GetTooltipInfoForStatus(this);

        public Status(GameTime gameTime, StatusType type, Unit instigator, Unit host, int additionFrame, int removalFrame)
        {
            _gameTime = gameTime;
            Type = type;
            Instigator = instigator;
            Host = host;
            AdditionFrame = additionFrame;
            RemovalFrame = removalFrame;
        }
        
        public Status(GameTime gameTime, StatusType type, Unit instigator, Unit host, int duration = -1)
        {
            _gameTime = gameTime;
            Type = type;
            Instigator = instigator;
            Host = host;
            AdditionFrame = _gameTime.Frame;
            RemovalFrame = AdditionFrame + duration;
        }
        
        public void OnAdd() => Type.OnAdd(this);

        public void OnUpdate()
        {
            Type.OnUpdate(this);
            if (FramesLeft == 0)
                Remove();
        }

        public void OnRemove() => Type.OnRemove(this);

        public void Restart(int newDuration)
        {
            if (RemovalFrame == -1  || FramesLeft > newDuration)
                return;
            RemovalFrame = _gameTime.Frame + newDuration;
        }

        public StatusSaveData ToSaveData()
        {
            return new StatusSaveData(Type.name, Instigator.Id, AdditionFrame, RemovalFrame);
        }

        private void Remove()
        {
            Host.Statuses.RemoveStatus(Type);
        }
    }

    public interface IStatusInfo
    {
        public StatusType Type { get; }
        public Unit Instigator { get; }
        public Unit Host { get; }
        public int AdditionFrame { get; }
        public int RemovalFrame { get; }
        public int FramesSinceAddition { get; }
        public bool IsLocked { get; }
        public int FramesLeft { get; }
        public string DisplayDescription { get; }
        public TooltipInfo TooltipInfo { get; }
    }
}