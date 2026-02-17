using System;
using Gameplay.Data.Statuses;
using UnityEngine;

namespace Gameplay.Units.View.StatusRendering
{
    public class StatusRenderer : MonoBehaviour
    {
        private StatusRendererFactory Factory { get; set; }
        public IStatusInfo Status { get; private set; }

        public void Init(StatusRendererFactory factory)
        {
            if (Factory != null)
                throw new Exception("StatusRenderer is already initialized");
            Factory = factory;
        }

        public virtual void OnAdd(IStatusInfo status)
        {
            Status = status;
        }

        public virtual void OnRemove()
        {
            Status = null;
        }

        public void Release() => Factory.ReleaseStatusRenderer(this);
    }
}