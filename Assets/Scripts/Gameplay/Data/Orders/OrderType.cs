using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gameplay.UI;
using Gameplay.Units;
using Localization;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Orders
{
    public abstract class OrderType : ScriptableObject
    {
        [SerializeField] private Localizer _localizer;
        
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea(4, 10)] private string _rawDisplayDescription;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _hotkeyAlias;

        protected Localizer Localizer => _localizer;
        
        public string DisplayName => Localizer.Translate(_displayName);
        public string RawDisplayDescription => Localizer.Translate(_rawDisplayDescription);
        public Sprite Icon => _icon;
        public string HotkeyAlias => _hotkeyAlias;

        public virtual string DisplayDescription => RawDisplayDescription;
        
        public virtual string DisplayType => Localizer.Translate("order");

        public TooltipInfo TooltipInfo => new TooltipInfo(_icon, DisplayName, DisplayType, DisplayDescription);
        
        public abstract TargetRequirement TargetRequirement { get; }
        
        public virtual bool IsValidForSmartOrder(OrderTarget target) => false;

        public bool CanBeIssued(Order order) =>
            IsTargetValid(order.Actor, order.Target, out _) && IsActorValid(order.Actor, out _);

        public virtual bool IsTargetValid(Unit actor, OrderTarget target, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public virtual bool IsActorValid(Unit actor, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public virtual bool MustBeCanceled(Order order)
        {
            if ( ! order.CanBeIssued())
                return true;
            if (order.Type.TargetRequirement == TargetRequirement.Unit && ! order.Target.Unit)
                return true;
            if (order.Target.Unit && ! order.Target.Unit.Visibility.CanBeTargetedBy(order.Actor))
                return true;
            return false;
        }

        public async UniTask CarryOut(Order order, CancellationToken ct)
        {
            try
            {
                await CarryOutBody(order, ct);
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                Dispose(order);
            }
        }

        protected abstract UniTask CarryOutBody(Order order, CancellationToken ct);
        
        protected abstract void Dispose(Order order);
    }

    public enum TargetRequirement
    {
        None,
        Point,
        Unit,
        PointOrUnit
    }
}