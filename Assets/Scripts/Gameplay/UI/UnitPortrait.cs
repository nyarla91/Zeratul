using Gameplay.Player;
using Gameplay.Units;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class UnitPortrait : MonoBehaviour
    {
        [SerializeField] private Image _portraitImage;
        [SerializeField] private TMP_Text _indexText;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _highlightedMaterial;
        [SerializeField] private Material _selectedMaterial;
        [SerializeField] private Material _focusedMaterial;

        public Unit Unit { get; private set; }
        
        [Inject] private PlayerInput Input { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }

        public void Set(Unit unit, int index)
        {
            Unit = unit;
            _portraitImage.sprite = Unit.Type.SpriteMap?.Portrait;
            _indexText.text = (index + 1).ToString();
        }

        private void Update()
        {
            if ( ! Unit)
                return;
            Material material = _defaultMaterial;
            if (Unit.IsHighlighted)
                material = _highlightedMaterial;
            else if (Unit.IsFocused)
                material = _focusedMaterial;
            else if (Unit.IsSelected)
                material = _selectedMaterial;
            _portraitImage.material = material;
        }

        public void OnPointerEnter()
        {
            MouseTargeting.OverrideUnit = Unit;
        }

        public void OnPointerExit()
        {
            if (MouseTargeting.OverrideUnit == Unit)
                MouseTargeting.OverrideUnit = null;
        }
    }
}