using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;
using PlayerInput = Gameplay.Player.PlayerInput;

namespace Gameplay.UI
{
    public class OrderButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _beginDragEventIndex;
        [SerializeField] private int _endDragEventIndex;
        [SerializeField] private int _clickEventIndex;

        private OrderType _orderType;
        private InputAction _hotkey;
        
        [Inject] private PlayerInput PlayerInput { get; set; } 
        [Inject] private PlayerOrderTargetSelector TargetSelector { get; set; } 
        [Inject] private PlayerOrdersDispatcher Dispatcher { get; set; } 

        private void Awake()
        {
            _eventTrigger.triggers[_beginDragEventIndex].callback.AddListener(StartTargeting);
            _eventTrigger.triggers[_endDragEventIndex].callback.AddListener(IssueWithTarget);
            _eventTrigger.triggers[_clickEventIndex].callback.AddListener(IssueWithoutTarget);
        }

        private void StartTargeting(BaseEventData _)
        {
            if (Mouse.current.leftButton.isPressed)
                StartTargeting();
        }

        private void StartTargeting(InputAction.CallbackContext _) => StartTargeting();

        private void StartTargeting()
        {
            if (_orderType == null || _orderType.TargetRequirement == TargetRequirement.None)
                return;
            TargetSelector.StartTargeting(_orderType.TargetRequirement);
        }

        private void IssueWithTarget(BaseEventData _)
        {
            
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                IssueWithTarget();
        }

        private void IssueWithTarget(InputAction.CallbackContext _) => IssueWithTarget();

        private void IssueWithTarget()
        {
            
            if (_orderType == null || _orderType.TargetRequirement == TargetRequirement.None)
                return;
            OrderTarget target = TargetSelector.FinishTargeting();
            if (_orderType.TargetRequirement == TargetRequirement.Unit && target.Unit == null)
                return;
            Dispatcher.IssueOrderToSelection(_orderType, target);
        }

        private void IssueWithoutTarget(BaseEventData _)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                IssueWithoutTarget();
        }

        private void IssueWithoutTarget(InputAction.CallbackContext _) => IssueWithoutTarget();

        private void IssueWithoutTarget()
        {
            
            if (_orderType == null || _orderType.TargetRequirement != TargetRequirement.None)
                return;
            Dispatcher.IssueOrderToSelection(_orderType, default);
        }

        public void ApplyOrderType(OrderType orderType)
        {
            if (_orderType == orderType)
                return;
            
            _image.color = orderType == null ? Color.clear : Color.white;
            _image.sprite = orderType?.Icon;

            DisposeHotkey();
            _hotkey = PlayerInput.GetOrderHotkeyAction(orderType?.HotkeyAlias);
            if (_hotkey != null)
            {
                _hotkey.started += StartTargeting;
                _hotkey.canceled += IssueWithTarget;
                _hotkey.performed += IssueWithoutTarget;
            }
            
            _orderType = orderType;
        }

        private void DisposeHotkey()
        {
            if (_hotkey == null)
                return;
            _hotkey.started -= StartTargeting;
            _hotkey.canceled -= IssueWithTarget;
            _hotkey.performed -= IssueWithoutTarget;
        }
    }
}