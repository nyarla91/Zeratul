using System;
using System.Collections.Generic;
using System.Linq;
using Extentions.Pause;
using Gameplay.Data.Configs;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using TMPro;
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
        [SerializeField] private OrderErrorConfig _errors;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _hotkeyPrompt;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _beginDragEventIndex;
        [SerializeField] private int _endDragEventIndex;
        [SerializeField] private int _clickEventIndex;
        [SerializeField] private int _pointerEnterEventIndex;
        [SerializeField] private int _pointerExitEventIndex;

        private InputAction _hotkey;
        private bool _showTooltip;
        
        public OrderType OrderType { get; private set; }
        
        [Inject] private PlayerInput PlayerInput { get; set; } 
        [Inject] private PlayerOrderTargetSelector TargetSelector { get; set; } 
        [Inject] private PlayerOrdersDispatcher Dispatcher { get; set; } 
        [Inject] private PlayerSelection Selection { get; set; } 
        [Inject] private Tooltip Tooltip { get; set; } 
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private OrderErrorMessage OrderErrorMessage { get; set; }

        private void Awake()
        {
            _eventTrigger.triggers[_beginDragEventIndex].callback.AddListener(StartTargeting);
            _eventTrigger.triggers[_endDragEventIndex].callback.AddListener(IssueWithTarget);
            _eventTrigger.triggers[_clickEventIndex].callback.AddListener(IssueWithoutTarget);
            _eventTrigger.triggers[_pointerEnterEventIndex].callback.AddListener(StartShowingTooltip);
            _eventTrigger.triggers[_pointerExitEventIndex].callback.AddListener(HideToolip);
        }

        public void ApplyOrderType(OrderType orderType)
        {
            if ( ! CanDisplayOrder(orderType))
                orderType = null;
            if (OrderType == orderType)
                return;
            
            _image.color = orderType == null ? Color.clear : Color.white;
            _image.sprite = orderType?.Icon;

            DisposeHotkey();
            _hotkey = orderType ? PlayerInput.GetOrderHotkeyAction(orderType.HotkeyAlias) : null;
            _hotkeyPrompt.text = _hotkey?.GetBindingDisplayString() ?? "";
            
            if (_hotkey != null)
            {
                _hotkey.started += StartTargeting;
                _hotkey.canceled += IssueWithTarget;
                _hotkey.performed += IssueWithoutTarget;
            }
            
            OrderType = orderType;
        }

        private void StartTargeting(BaseEventData _)
        {
            if (Mouse.current.leftButton.isPressed)
                StartTargeting();
        }

        private void StartTargeting(InputAction.CallbackContext _) => StartTargeting();

        private void StartTargeting()
        {
            if (GamePause.IsPaused)
                return;
            if (Selection.IsUncontrollableSelected)
                return;
            if (OrderType == null || OrderType.TargetRequirement == TargetRequirement.None)
                return;
            if ( ! Dispatcher.CanIssueWithoutTarget(OrderType, out string errorMessage))
            {
                OrderErrorMessage.Show(errorMessage);
                return;
            }
            TargetSelector.StartTargeting(OrderType);
        }

        private void IssueWithTarget(BaseEventData _)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                IssueWithTarget();
        }

        private void IssueWithTarget(InputAction.CallbackContext _) => IssueWithTarget();

        private void IssueWithTarget()
        {
            if (GamePause.IsPaused)
                return;
            if (Selection.IsUncontrollableSelected)
                return;
            if (OrderType == null || OrderType.TargetRequirement == TargetRequirement.None)
                return;
            if ( ! TargetSelector.IsTargeting)
                return;
            OrderTarget target = TargetSelector.FinishTargeting();
            if ( ! Dispatcher.CanIssueWithTarget(OrderType, target, out string errorMessage))
            {
                OrderErrorMessage.Show(errorMessage);
                return;
            }
            if (OrderType.TargetRequirement == TargetRequirement.Unit && target.Unit == null)
                return;
            Dispatcher.IssueOrderToSelection(OrderType, target);
        }

        private void IssueWithoutTarget(BaseEventData _)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                IssueWithoutTarget();
        }

        private void IssueWithoutTarget(InputAction.CallbackContext _) => IssueWithoutTarget();

        private void IssueWithoutTarget()
        {
            if (GamePause.IsPaused)
                return;
            if (Selection.IsUncontrollableSelected)
                return;
            if (OrderType == null || OrderType.TargetRequirement != TargetRequirement.None)
                return;
            if ( ! Dispatcher.CanIssueWithoutTarget(OrderType, out string errorMessage))
            {
                OrderErrorMessage.Show(errorMessage);
                return;
            }
            Dispatcher.IssueOrderToSelection(OrderType, default);
        }

        private void StartShowingTooltip(BaseEventData _)
        {
            _showTooltip = true;
        }

        private void HideToolip(BaseEventData _)
        {
            _showTooltip = false;
            Tooltip.Hide();
        }

        private bool CanDisplayOrder(OrderType orderType)
        {
            if ( ! orderType)
                return false;
            
            HashSet<Unit> units = Selection.SelectedUnits
                .Where(u => u.Type.AvailableOrders.Contains(orderType))
                .ToHashSet();
            
            return units
                .Any(orderType.CanBeDisplayed);
        }

        private void Update()
        {
            if (GamePause.IsPaused)
            {
                HideToolip(null);
            }
            if (_showTooltip && OrderType)
                Tooltip.Show(OrderType.TooltipInfo);
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