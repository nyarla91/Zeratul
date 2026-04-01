using System;
using Gameplay.Data.Statuses;
using UniRx;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public abstract class StatusRenderer : MonoBehaviour
    {
        [SerializeField] private bool _alwaysVisible;
        
        private StatusRendererFactory Factory { get; set; }
        public IStatusInfo Status { get; private set; }

        protected bool IsVisible => _alwaysVisible || (Status?.Host.VisibleToPlayer ?? false);
        
        public void Init(StatusRendererFactory factory)
        {
            if (Factory != null)
                throw new Exception("StatusRenderer is already initialized");
            Factory = factory;

            this.ObserveEveryValueChanged(s => s.IsVisible)
                .Subscribe(UpdateVisibility);
            UpdateVisibility(IsVisible);
        }

        public virtual void OnAdd(IStatusInfo status)
        {
            Status = status;
            UpdateVisibility(IsVisible);
        }

        public virtual void OnRemove()
        {
            Status = null;
        }

        public void Release() => Factory.ReleaseStatusRenderer(this);

        protected abstract void UpdateVisibility(bool isVisible);
    }
}