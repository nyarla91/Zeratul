using Gameplay.UI;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Orders
{
    public abstract class OrderType : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(4, 10)] private string _rawDisplayDescription;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _hotkeyAlias;

        public string DisplayName => _displayName;
        public string RawDisplayDescription => _rawDisplayDescription;
        public Sprite Icon => _icon;
        public string HotkeyAlias => _hotkeyAlias;

        public virtual string DisplayDescription => RawDisplayDescription;
        
        public virtual string DisplayType => "Order";
        
        public TooltipInfo TooltipInfo => new TooltipInfo(_icon, DisplayName, DisplayType, DisplayDescription);
        
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