using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    public abstract class OrderType : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _hotkeyAlias;

        public Sprite Icon => _icon;
        public string HotkeyAlias => _hotkeyAlias;

        public abstract TargetRequirement TargetRequirement { get; }
        
        public virtual bool IsValidForSmartOrder(OrderTarget target) => false;

        public abstract void OnProceed(Order order);

        public abstract void OnUpdate(Order order);

        public abstract void Dispose(Order order);

        public virtual bool IsCompleted(Order order) => false;
        
        public virtual bool CanBeIssued(Order order) => true;

        public void Complete(Order order)
        {
            if (order.Actor.Orders.CurrentOrder.Type != this)
                return;
            order.Actor.Orders.CompleteCurrentOrder();
        }
    }

    public enum TargetRequirement
    {
        None,
        Point,
        Unit,
        PointOrUnit
    }
}