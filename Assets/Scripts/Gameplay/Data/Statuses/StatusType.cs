using Extentions.Pause;
using Gameplay.UI;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    public abstract class StatusType : ScriptableObject
    {
        [SerializeField] private bool _display;
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(4, 10)] private string _rawDisplayDescription;

        public bool Display => _display;
        public Sprite DisplayIcon => _displayIcon;
        public string DisplayName => _displayName;
        public string RawDisplayDescription => _rawDisplayDescription;

        public TooltipInfo GetTooltipInfoForStatus(Status status)
        {
            return new TooltipInfo(DisplayIcon, DisplayName, "Status", status.DisplayDescription);
        }

        public abstract void OnAdd(Status status);
        
        public abstract void OnUpdate(Status status);
        
        public abstract void OnRemove(Status status);
    }

    public class Status : IStatusInfo
    {
        public StatusType Type { get; }
        public Unit Instigator { get; }
        public Unit Host { get; }
        public int AdditionFrame { get; private set; }
        public int RemovalFrame { get; private set; }
        public int CurrentFrame { get; private set; }

        public int FramesLeft => RemovalFrame - CurrentFrame;
        public int Duration =>  RemovalFrame - AdditionFrame;
        
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
    
    public interface IStatusInfo
    {
        public StatusType Type { get; }
        public Unit Instigator { get; }
        public Unit Host { get; }
        public int AdditionFrame { get; }
        public int RemovalFrame { get; }
        public int CurrentFrame { get; }
        public int FramesLeft { get; }
        public int Duration { get; }
        public string DisplayDescription { get; }
        public TooltipInfo TooltipInfo { get; }
    }
}