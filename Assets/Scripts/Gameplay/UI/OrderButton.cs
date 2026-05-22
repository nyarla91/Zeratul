using System.Collections.Generic;
using System.Linq;
using Extentions;
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
        [SerializeField] private CanvasGroup _displayGroup;
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

        private bool IsVisible => OrderType && _displayGroup.interactable;
        
        public OrderType OrderType { get; private set; }
        
        [Inject] private PlayerInput PlayerInput { get; set; } 
        [Inject] private PlayerOrderTargeter Targeter { get; set; } 
        [Inject] private PlayerOrdersDispatcher Dispatcher { get; set; } 
        [Inject] private PlayerSelection Selection { get; set; } 
        [Inject] private Tooltip Tooltip { get; set; } 
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private OrderErrorMessage OrderErrorMessage { get; set; }

        private void Awake()
        {
            _eventTrigger.triggers[_clickEventIndex].callback.AddListener(HandleActivation);
            _eventTrigger.triggers[_pointerEnterEventIndex].callback.AddListener(StartShowingTooltip);
            _eventTrigger.triggers[_pointerExitEventIndex].callback.AddListener(HideTooltip);
        }

        public void ApplyOrderType(OrderType orderType)
        {
            if (OrderType == orderType)
                return;
            
            _image.color = orderType == null ? Color.clear : Color.white;
            _image.sprite = orderType?.Icon;

            DisposeHotkey();
            _hotkey = orderType ? PlayerInput.GetOrderHotkeyAction(orderType.HotkeyAlias) : null;
            _hotkeyPrompt.text = _hotkey?.GetBindingDisplayString() ?? "";
            
            if (_hotkey != null)
            {
                _hotkey.performed += HandleActivation;
            }
            
            OrderType = orderType;
        }

        private void HandleActivation(BaseEventData _)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                HandleActivation();
        }

        private void HandleActivation(InputAction.CallbackContext _) => HandleActivation();

        private void HandleActivation()
        {
            if (GamePause.IsPaused)
                return;
            if (Selection.IsUncontrollableSelected)
                return;
            if (OrderType == null)
                return;
            if ( ! IsVisible)
                return;
            if ( ! Dispatcher.CanIssueWithoutTarget(OrderType, out string errorMessage))
            {
                OrderErrorMessage.Show(errorMessage);
                return;
            }

            if (OrderType.TargetRequirement == TargetRequirement.None)
            {
                Dispatcher.IssueOrderToSelection(OrderType, default);
                Targeter.CancelTargeting();
            }
            else
            {
                Targeter.StartTargeting(OrderType);
            }
        }

        private void StartShowingTooltip(BaseEventData _)
        {
            if (IsVisible)
                _showTooltip = true;
        }

        private void HideTooltip(BaseEventData _)
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
            if (OrderType && CanDisplayOrder(OrderType))
                _displayGroup.ToggleOn();
            else
                _displayGroup.ToggleOff();

            if (GamePause.IsPaused)
            {
                HideTooltip(null);
            }
            if (_showTooltip && OrderType)
                Tooltip.Show(OrderType.TooltipInfo);
        }

        private void DisposeHotkey()
        {
            if (_hotkey == null)
                return;
            _hotkey.performed -= HandleActivation;
        }
    }
}