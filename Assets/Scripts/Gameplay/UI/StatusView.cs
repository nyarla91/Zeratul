using _Core;
using Gameplay.Units;
using Settings.Localization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _icon;
        [SerializeField] private AnimationCurve _alphaPerFramesLeft;
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
            _icon.color = Color.white.WithA(_alphaPerFramesLeft.Evaluate(status.FramesLeft));
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
            if (_showTooltip && _currentStatus != null)
                Tooltip.Show(GetTooltipInfoForStatus(_currentStatus));
        }

        private TooltipInfo GetTooltipInfoForStatus(IStatusInfo currentStatus)
        {
            Sprite icon = currentStatus.Type.DisplayIcon;
            string label = _localizer.Translate(currentStatus.Type.DisplayName);
            string sublabel = _localizer.Translate("status");
            string description = GetDescriptionForStatus(currentStatus);
            return new TooltipInfo(icon, label, sublabel, description);
        }

        private string GetDescriptionForStatus(IStatusInfo currentStatus)
        {
            string description = _localizer.Translate(currentStatus.Type.DisplayDescription);
            if (currentStatus.FramesLeft > 3)
            { 
                string timeLeft = _localizer.Translate("status-duration");
                description += "\n" + timeLeft.Replace("#", currentStatus.FramesLeft.FramesToSeconds());
            }
            return description;
        }
    }
}