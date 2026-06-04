using System.Linq;
using _Core;
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
    public class TargeterView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showDelay;
        [SerializeField] private Image _orderIcon;
        [SerializeField] private TMP_Text _orderName;
        [SerializeField] private TMP_Text _validationMessage;
        [Space]
        [SerializeField] private AoeVariant _rangeVariant;
        [SerializeField] private AoeVariant _targetVariant;

        private float _targetingTime;
        private AoeView _rangeAoe;
        private AoeView _targetAoe;

        private OrderType CurrentOrder => Targeter.CurrentOrder; 
        private AbilityOrder CurrentAbilityOrder => CurrentOrder as AbilityOrder; 
        
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] public PlayerOrderTargeter Targeter { get; }
        [Inject] public PlayerOrdersDispatcher OrdersDispatcher { get; }
        [Inject] private PoolFactory<AoeView> AoeFactory { get; set; }

        private void Awake()
        {
            _rangeAoe = AoeFactory.Get();
            _targetAoe = AoeFactory.Get();
        }

        public void CancelTargeting() => Targeter.CancelTargeting();

        private void Update()
        {
            _targetingTime = CurrentOrder ? (_targetingTime + Time.deltaTime) : 0;

            if (_targetingTime < _showDelay)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
                _rangeAoe.Hide();
                _targetAoe.Hide();
                return;
            }
            
            Unit[] actors = Selection.SelectedUnits.Where(u => u.Type.AvailableOrders.Contains(CurrentOrder)).ToArray();
            
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
            _orderName.text = CurrentOrder.DisplayName;
            _orderIcon.sprite = CurrentOrder.Icon;
            OrdersDispatcher.CanIssueWithTarget(CurrentOrder, Targeter.CurrentTarget, out string errorMessage);
            _validationMessage.text = errorMessage;
            
            UpdateEllipses(actors);
        }

        private void UpdateEllipses(Unit[] actors)
        {
            if ( ! CurrentAbilityOrder)
            {
                _rangeAoe.Hide();
                _targetAoe.Hide();
                return;
            }
            _rangeAoe.Show();
            float radius = CurrentAbilityOrder.AbilityType.MaxDistance;
            _rangeAoe.Set(_rangeVariant.WithRadius(radius));
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Unit closestUnit = actors.MinElement(a => Isometry.Distance(a.Position, mousePosition));
            _rangeAoe.Move(closestUnit.Position);

            radius = CurrentAbilityOrder.AoeEllipseRadius;
            if (radius == 0 || (CurrentAbilityOrder.TargetRequirement == TargetRequirement.Unit && ! Targeter.CurrentTarget.Unit))
            {
                _targetAoe.Hide();
                return;
            }
            _targetAoe.Show();
            _targetAoe.Set(_targetVariant.WithRadius(radius));
            Vector3 position = Targeter.CurrentTarget.Unit ? Targeter.CurrentTarget.Unit.Position : Targeter.CurrentTarget.Point;
            _targetAoe.Move(position);
        }
    }
}