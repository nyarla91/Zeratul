using Gameplay.Data.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextFormattingConfig _config;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _sublabel;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private RectTransform _descriptionRect;
        [SerializeField] private float _baseHeight;
        [SerializeField] private Vector2 _preferredPivot;

        private bool IsShown => _canvasGroup.alpha > 0;
        private Vector2 MousePosition => Mouse.current.position.ReadValue();
        private bool CanFitRight => MousePosition.x + _rect.rect.width <= Screen.width;
        private bool CanFitLeft => MousePosition.x - _rect.rect.width > 0;
        private bool CanFitUpwards => MousePosition.y + _rect.rect.height <= Screen.height;
        private bool CanFitDownwards => MousePosition.y - _rect.rect.height > 0;

        private void Start()
        {
            _canvasGroup.alpha = 0;
        }

        public void Show(TooltipInfo info)
        {
            _canvasGroup.alpha = 1;
            _icon.sprite = info.Icon;
            _label.text = info.Label;
            _sublabel.text = info.Sublabel;
            _description.text = _config.Format(info.Description);
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        private void Update()
        {
            float height = _descriptionRect.rect.height + _baseHeight;
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, height);
            
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            
            float xPivot = CanFitRight ? (CanFitLeft ? _preferredPivot.x : 0) : 1;
            float yPivot = CanFitUpwards ? (CanFitDownwards ? _preferredPivot.y : 0) : 1;
            _rect.pivot = new Vector2(xPivot, yPivot);
            
            _rect.anchoredPosition = mousePosition;
        }
    }

    public struct TooltipInfo
    {
        public Sprite Icon { get; }
        public string Label { get; }
        public string Sublabel { get; }
        public string Description { get; }

        public TooltipInfo(Sprite icon, string label, string sublabel, string description)
        {
            Icon = icon;
            Label = label;
            Sublabel = sublabel;
            Description = description;
        }
    }
}