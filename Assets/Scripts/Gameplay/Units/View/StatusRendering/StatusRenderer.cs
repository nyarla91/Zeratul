using System;
using Extentions;
using UniRx;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public abstract class StatusRenderer : PoolElement<StatusRenderer>
    {
        [SerializeField] private DisplayBehaviour _playerDisplayBehaviour;
        [SerializeField] private DisplayBehaviour _enemyDisplayBehaviour;
        [SerializeField] private bool _visibleInFogOfWar;
        [SerializeField] private bool _ignoreLocked;
        
        public IStatusInfo Status { get; set; }

        protected bool IsVisible
        {
            get
            {
                if (Status == null)
                    return false;
                
                Unit host = Status.Host;
                
                if ( ! _visibleInFogOfWar && ! host.IsVisibleToPlayer)
                    return false;
                
                if ( ! _ignoreLocked && Status.IsLocked)
                    return false;
                
                DisplayBehaviour displayBehaviour = host.Alliance.IsFriendly(Owner.Player)
                    ? _playerDisplayBehaviour
                    : _enemyDisplayBehaviour;

                return displayBehaviour switch
                {
                    DisplayBehaviour.Always => true,
                    DisplayBehaviour.WhenHighlighted => host.IsHighlighted,
                    DisplayBehaviour.WhenSelected => host.IsSelected,
                    DisplayBehaviour.WhenHighlightedOrSelected => host.IsHighlighted || host.IsSelected,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public override void Init(PoolFactory<StatusRenderer> factory, GameObject prefab)
        {
            base.Init(factory, prefab);
            this.ObserveEveryValueChanged(s => s.IsVisible)
                .Subscribe(UpdateVisibility);
            UpdateVisibility(IsVisible);
        }

        public override void OnSpawn()
        {
            UpdateVisibility(IsVisible);
        }

        protected override void OnDespawn()
        {
            Status = null;
        }
        
        protected abstract void UpdateVisibility(bool isVisible);

        private enum DisplayBehaviour
        {
            Always,
            WhenHighlighted,
            WhenSelected,
            WhenHighlightedOrSelected
        }
    }
}