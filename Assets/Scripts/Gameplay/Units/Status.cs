using Gameplay.Data.Statuses;
using Save.Data.Units;

namespace Gameplay.Units
{
    public class Status : IStatusInfo
    {
        private readonly GameTime _gameTime;
        
        public StatusType Type { get; }
        public Unit Instigator { get; }
        public Unit Host { get; }
        public int AdditionFrame { get; private set; }
        public int RestartFrame { get; private set; }
        public int RemovalFrame { get; private set; }

        public int FramesSinceAddition => _gameTime.Frame - AdditionFrame;
        public bool IsLocked => Type.IsLocked(this);

        public int FramesLeft => RemovalFrame - _gameTime.Frame;

        public Status(GameTime gameTime, StatusType type, Unit instigator, Unit host, int additionFrame, int removalFrame)
        {
            _gameTime = gameTime;
            Type = type;
            Instigator = instigator;
            Host = host;
            AdditionFrame = additionFrame;
            RestartFrame = additionFrame;
            RemovalFrame = removalFrame;
        }
        
        public Status(GameTime gameTime, StatusType type, Unit instigator, Unit host, int duration = -1)
        {
            _gameTime = gameTime;
            Type = type;
            Instigator = instigator;
            Host = host;
            AdditionFrame = _gameTime.Frame;
            RestartFrame = _gameTime.Frame;
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
            RestartFrame = _gameTime.Frame;
            RemovalFrame = _gameTime.Frame + newDuration;
        }

        public StatusSaveData ToSaveData()
        {
            return new StatusSaveData(Type.name, Instigator.Id, AdditionFrame, RemovalFrame);
        }

        public void Remove()
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
        public int RestartFrame { get; }
        public int RemovalFrame { get; }
        public int FramesSinceAddition { get; }
        public bool IsLocked { get; }
        public int FramesLeft { get; }
    }
}