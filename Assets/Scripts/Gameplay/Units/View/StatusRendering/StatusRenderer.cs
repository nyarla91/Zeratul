using System;
using Gameplay.Data.Statuses;
using UniRx;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public abstract class StatusRenderer : PoolElement<StatusRenderer>
    {
        [SerializeField] private bool _alwaysVisible;
        
        public IStatusInfo Status { get; set; }

        protected bool IsVisible => _alwaysVisible || (Status?.Host.IsVisibleToPlayer ?? false);

        public override void Init(PoolFactory<StatusRenderer> factory)
        {
            base.Init(factory);
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
    }
}