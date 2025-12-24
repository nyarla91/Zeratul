using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Gameplay.UI
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _sublabel;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private RectTransform _descriptionRect;
        [SerializeField] private float _baseHeight;

        private bool IsShown => _canvasGroup.alpha > 0;

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
            _description.text = info.Description;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        private void Update()
        {
            float height = _descriptionRect.rect.height + _baseHeight;
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, height);
            
            _rect.anchoredPosition = Mouse.current.position.ReadValue();
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