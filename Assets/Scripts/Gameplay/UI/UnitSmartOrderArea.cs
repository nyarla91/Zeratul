using Extentions.Pause;
using Gameplay.Data.Orders;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.UI
{
    public class UnitSmartOrderArea : MonoBehaviour
    {
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _pointerDownEventIndex;
        
        [Inject] private PlayerOrdersDispatcher OrdersDispatcher { get; set; }
        [Inject] private PlayerOrderTargetSelector TargetSelector { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        
        private void Awake()
        {
            _eventTrigger.triggers[_pointerDownEventIndex].callback.AddListener(IssueSmartOrder);
        }

        private void IssueSmartOrder(BaseEventData _)
        {
            if (GamePause.IsPaused || ! Mouse.current.rightButton.wasPressedThisFrame)
                return;
            const TargetRequirement requirement = TargetRequirement.PointOrUnit;
            OrdersDispatcher.IssueSmartOrderToSelection(TargetSelector.GetTargetForRequirement(requirement));
        }
    }
}