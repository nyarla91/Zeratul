using System;
using Extentions;
using Gameplay.Data.Orders;
using Gameplay.Data.Statuses;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _fillIcon;
        [SerializeField] private RectMask2D _fillMask;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _pointerEnterEventIndex;
        [SerializeField] private int _pointerExitEventIndex;
        
        private IStatusInfo _currentStatus;
        private bool _showTooltip;
        
        [Inject] private Tooltip Tooltip { get; set; }
        
        private void Awake()
        {
            _eventTrigger.triggers[_pointerEnterEventIndex].callback.AddListener(StartShowingTooltip);
            _eventTrigger.triggers[_pointerExitEventIndex].callback.AddListener(HideTooltip);
        }

        public void UpdateView(IStatusInfo status)
        {
            _currentStatus = status;
            if (_currentStatus == null || ! _currentStatus.Type.Display)
            {
                _canvasGroup.alpha = 0;
                return;
            }
            _canvasGroup.alpha = 1;
            _icon.sprite = _currentStatus.Type.DisplayIcon;
            _fillIcon.sprite = _currentStatus.Type.DisplayIcon;
            float fillPercent = _currentStatus.FramesLeft > 0 ? (float) _currentStatus.FramesLeft / _currentStatus.Duration : 1;
            _fillMask.Fill(true, false, fillPercent);
        }

        private void StartShowingTooltip(BaseEventData _)
        {
            _showTooltip = true;
        }

        private void HideTooltip(BaseEventData _)
        {
            _showTooltip = false;
            Tooltip.Hide();
        }

        private void Update()
        {
            Debug.Log($"{_showTooltip} :::{_currentStatus}");
            if (_showTooltip && _currentStatus != null)
                Tooltip.Show(_currentStatus.TooltipInfo);
        }
    }
}