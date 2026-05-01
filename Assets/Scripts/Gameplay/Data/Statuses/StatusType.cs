using Gameplay.UI;
using Gameplay.Units;
using Gameplay.Units.View;
using Gameplay.Units.View.StatusRendering;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    public abstract class StatusType : ScriptableObject
    {
        [SerializeField] private bool _useLock;
        [SerializeField] private bool _display;
        [SerializeField] private Sprite _displayIcon;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(4, 10)] private string _rawDisplayDescription;
        [SerializeField] private StatusRenderer[] _rendererPrefabs;

        public bool Display => _display;
        public Sprite DisplayIcon => _displayIcon;
        public string DisplayName => _displayName;
        public string RawDisplayDescription => _rawDisplayDescription;
        public StatusRenderer[] RendererPrefabs => _rendererPrefabs;

        public TooltipInfo GetTooltipInfoForStatus(Status status)
        {
            return new TooltipInfo(DisplayIcon, DisplayName, "Status", status.DisplayDescription);
        }

        public abstract void OnAdd(Status status);
        
        public abstract void OnUpdate(Status status);
        
        public abstract void OnRemove(Status status);

        public bool IsLocked(Status status) => _useLock && status.Host.Abilities.IsLocked;
    }
}