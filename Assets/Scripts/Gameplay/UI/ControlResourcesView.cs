using System;
using System.ComponentModel;
using System.Linq;
using Gameplay.Data.Validator;
using Gameplay.Player;
using Gameplay.Units;
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
        [SerializeField] private UnitValidatorGroup _displayValidator;
        [SerializeField] private TMP_Text _extraReserve;
        [SerializeField] private Sprite _occupiedSprite;
        [SerializeField] private Sprite _availableSprite;
        [SerializeField] private Sprite _availableReserveSprite;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _highlightColor;
        [SerializeField] private Color _extraReserveColor;
        [SerializeField] private Color _extraCostColor;
        [SerializeField] private UnitValidatorGroup _highlightValidator;
        
        [Inject] private PlayerControlResources ControlResources { get; set; }
        [Inject] private PlayerMouseTargeting PlayerMouseTargeting { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }

        private void Awake()
        {
            _canvasGroup.alpha = 0;

            this.UpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(1))
                .Where(_ => UnitPool.PlayerUnits.Any(u => _displayValidator.IsValid(u, u)))
                .Take(1)
                .Subscribe(_ => _canvasGroup.alpha = 1);
            
            this.UpdateAsObservable()
                .Where(_ => _canvasGroup.alpha.Equals(1))
                .Subscribe(_ => UpdateSlots());
        }

        private void UpdateSlots()
        {
            int highlightStart = -1;
            int highlightLength = 0;
            Unit highlightedUnit = PlayerMouseTargeting.Unit;
            bool highlight = PlayerMouseTargeting.Unit && _highlightValidator.IsValid(highlightedUnit, highlightedUnit); 
            if (highlight)
            {
                highlightStart = highlightedUnit.Alliance.OwnedByPlayer ? 0 : ControlResources.OccupiedSlots;
                highlightLength = highlightedUnit.Type.ControlWorth;
            }
            
            for (int i = 0; i < _slots.Length; i++)
            {
                if (i >= ControlResources.Slots)
                {
                    _slots[i].gameObject.SetActive(false);
                    continue;
                }
                _slots[i].gameObject.SetActive(true);

                if (i < ControlResources.OccupiedSlots)
                    _slots[i].sprite = _occupiedSprite;
                else if (i < ControlResources.OccupiedSlots + ControlResources.Reserve)
                    _slots[i].sprite = _availableReserveSprite;
                else
                    _slots[i].sprite = _availableSprite;
                
                bool highlightSlot = i >= highlightStart && i < highlightStart + highlightLength;
                _slots[i].color = highlightSlot ? _highlightColor : _defaultColor;
            }

            int extra = ControlResources.ExtraReserve;
            Color extraColor = _extraReserveColor;
            if (highlight && highlightedUnit.Alliance.OwnedByEnemy && highlightedUnit.Type.ControlWorth > ControlResources.AvailableSlots)
            {
                extra = highlightedUnit.Type.ControlWorth - ControlResources.AvailableSlots;
                extraColor = _extraCostColor;
            }
            
            _extraReserve.gameObject.SetActive(extra > 0);
            _extraReserve.text = $"+{extra}";
            _extraReserve.color = extraColor;
        }
    }
}