using System;
using System.Linq;
using Extentions;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using Gameplay.Visual;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class TargetSelectorView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showDelay;
        [SerializeField] private bool _enableTacticalPause;
        [SerializeField] private Image _orderIcon;
        [SerializeField] private TMP_Text _orderName;
        [SerializeField] private TMP_Text _validationMessage;
        [Space]
        [SerializeField] private float _ellipseThickness;
        [SerializeField] private Color _ellipseColor;

        private float _targetingTime;
        private RangeEllipse _rangeEllipse;
        private RangeEllipse _aoeEllipse;

        private OrderType CurrentOrder => Selector.CurrentOrder; 
        private AbilityOrder CurrentAbilityOrder => CurrentOrder as AbilityOrder; 
        
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] public PlayerOrderTargetSelector Selector { get; }
        [Inject] private RangeEllipseFactory RangeEllipseFactory { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Awake()
        {
            _rangeEllipse = RangeEllipseFactory.Get();
            _aoeEllipse = RangeEllipseFactory.Get();
        }

        private void Update()
        {
            _targetingTime = CurrentOrder ? (_targetingTime + Time.deltaTime) : 0;

            if (_targetingTime < _showDelay)
            {
                _canvasGroup.alpha = 0;
                _rangeEllipse.Hide();
                _aoeEllipse.Hide();
                TacticalPause.Unpause(this);
                return;
            }
            if (_enableTacticalPause)
                TacticalPause.Pause(this);
            
            Unit[] actors = Selection.SelectedUnits.Where(u => u.Type.AvailableOrders.Contains(CurrentOrder)).ToArray();
            
            _canvasGroup.alpha = 1;
            _orderName.text = CurrentOrder.DisplayName;
            _orderIcon.sprite = CurrentOrder.Icon;
            _validationMessage.text = GetValidationMessageText(actors);
            
            UpdateEllipses(actors);
        }

        private string GetValidationMessageText(Unit[] actors)
        {
            if (CurrentOrder.TargetRequirement == TargetRequirement.None)
                return null;
            if (CurrentOrder.TargetRequirement == TargetRequirement.Point)
                return null;
            if (CurrentOrder.TargetRequirement == TargetRequirement.Unit && ! Selector.CurrentTarget.Unit)
                return "Must target a unit";
            if ( ! Selector.CurrentTarget.Unit)
                return null;
            
            if ( ! CurrentAbilityOrder)
                return null;


            string invalidMessage = "";
            foreach (Unit actor in actors)
            {
                if (CurrentAbilityOrder.AbilityType.TargetValidators.IsValid(actor, Selector.CurrentTarget.Unit, out invalidMessage))
                    return null;
            }
            return invalidMessage;
        }

        private void UpdateEllipses(Unit[] actors)
        {
            if (!CurrentAbilityOrder)
            {
                _rangeEllipse.Hide();
                _aoeEllipse.Hide();
                return;
            }
            _rangeEllipse.Show();
            float radius = CurrentAbilityOrder.AbilityType.MaxDistance;
            _rangeEllipse.Set(radius, _ellipseThickness, _ellipseColor);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Unit closestUnit = actors.MinElement(a => Isometry.Distance(a.Position, mousePosition));
            _rangeEllipse.Move(closestUnit.Position);

            radius = CurrentAbilityOrder.AoeEllipseRadius;
            if (radius == 0 || (CurrentAbilityOrder.TargetRequirement == TargetRequirement.Unit && ! Selector.CurrentTarget.Unit))
            {
                _aoeEllipse.Hide();
                return;
            }
            _aoeEllipse.Show();
            _aoeEllipse.Set(radius, _ellipseThickness, _ellipseColor);
            Vector3 position = Selector.CurrentTarget.Unit ? Selector.CurrentTarget.Unit.Position : Selector.CurrentTarget.Point;
            _aoeEllipse.Move(position);
        }
    }
}