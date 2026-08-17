using System;
using _Core;
using Gameplay.Data;
using Gameplay.Data.Validator;
using Gameplay.Player;
using Gameplay.Units;
using Gameplay.Upgrades;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.UI
{
    public class ControlResourcesView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image[] _slots;
        [SerializeField] private Upgrade _upgradeRequired;
        [SerializeField] private TMP_Text _reserve;
        [SerializeField] private Sprite _availableSprite;
        [SerializeField] private Sprite _highlightedSprite;
        [SerializeField] private Color _reserveColor;
        [SerializeField] private Color _unavailableReserveColor;
        [SerializeField] private UnitValidatorGroup _highlightValidator;
        
        [Inject] private PlayerControlResources ControlResources { get; set; }
        [Inject] private PlayerOrderTargeter Targeter { get; set; }
        [Inject] private PlayerMouseTargeting PlayerMouseTargeting { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private UpgradeStorage UpgradeStorage { get; set; }

        private void Awake()
        {
            _canvasGroup.alpha = 0;

            this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(1))
                .Where(_ => UpgradeStorage.IsUpgradeResearched(Owner.Player, _upgradeRequired))
                .Take(1)
                .Subscribe(_ => _canvasGroup.alpha = 1);
            
            this.UpdateAsObservable()
                .Where(_ => _canvasGroup.alpha.Equals(1))
                .Subscribe(_ => UpdateView());
        }

        private void UpdateView()
        {
            Unit highlightedUnit = GetHighlightedUnit();
            UpdateSlots(highlightedUnit);
            UpdateReserve(highlightedUnit);
        }

        private void UpdateSlots(Unit highlightedUnit)
        {
            int availableSlots = ControlResources.AvailableSlots;
            int highlightedSlots = highlightedUnit ? (Mathf.Min(availableSlots, highlightedUnit.Type.ControlSlots)) : 0;
            
            for (int i = 0; i < _slots.Length; i++)
            {
                Image slot = _slots[i];
                if (i >= availableSlots)
                {
                    slot.gameObject.SetActive(false);
                    continue;
                }
                slot.gameObject.SetActive(true);
                if (i < highlightedSlots)
                    slot.sprite = _highlightedSprite;
                else
                    slot.sprite = _availableSprite;
            }
        }

        private void UpdateReserve(Unit highlightedUnit)
        {
            int reserve = ControlResources.Reserve;
            int reserveRequired = highlightedUnit?.Type.ControlCost ?? 0;
            _reserve.color = (reserveRequired > reserve) ? _unavailableReserveColor : _reserveColor;
            _reserve.text = reserveRequired > 0 ? $"{reserve}/{reserveRequired}" : $"{reserve}";
        }

        private Unit GetHighlightedUnit()
        {
            if ( ! (Targeter.CurrentOrder?.HighlightControl ?? false))
                return null;
            Unit result = PlayerMouseTargeting.Unit;
            if (result == null || _highlightValidator.IsInvalid(result, result))
                return null;
            return result;
        }
    }
}