using _Core.Pause;
using Gameplay.Data.Orders;
using Gameplay.Player;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.UI
{
    public class UnitSmartOrderArea : MonoBehaviour
    {
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _pointerClickEventIndex;
        
        [Inject] private ClickArea ClickArea { get; set; }
        [Inject] private PlayerOrdersDispatcher OrdersDispatcher { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }
        [Inject] private PlayerOrderTargeter Targeter { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        
        private void Awake()
        {
            Targeter.ObserveEveryValueChanged(t => t.IsTargeting)
                .Subscribe(UpdateSubscriptions);
        }

        private void UpdateSubscriptions(bool isTargeting)
        {
            if (isTargeting)
            {
                ClickArea.RightClicked -= IssueSmartOrder;
            }
            else
            {
                ClickArea.RightClicked += IssueSmartOrder;
            }
        }

        private void IssueSmartOrder()
        {
            if (GamePause.IsPaused || ! Mouse.current.rightButton.wasReleasedThisFrame)
                return;
            const TargetRequirement requirement = TargetRequirement.PointOrUnit;
            OrdersDispatcher.IssueSmartOrderToSelection(MouseTargeting.GetTargetForRequirement(requirement));
        }
    }
}