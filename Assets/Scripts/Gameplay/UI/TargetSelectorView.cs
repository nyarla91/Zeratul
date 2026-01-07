using System.Linq;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class TargetSelectorView : MonoBehaviour
    {
        [SerializeField] private PlayerOrderTargetSelector _selector;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showDelay;
        [SerializeField] private Image _orderIcon;
        [SerializeField] private TMP_Text _orderName;
        [SerializeField] private TMP_Text _validationMessage;

        private float _targetingTime;

        private OrderType CurrentOrder => _selector.CurrentOrder; 
        
        [Inject] private PlayerSelection Selection { get; set; }
        
        private void Update()
        {
            _targetingTime = CurrentOrder ? (_targetingTime + Time.deltaTime) : 0;

            if (_targetingTime < _showDelay)
            {
                _canvasGroup.alpha = 0;
                return;
            }
            _canvasGroup.alpha = 1;
            _orderName.text = CurrentOrder.DisplayName;
            _orderIcon.sprite = CurrentOrder.Icon;
            _validationMessage.text = GetValidationMessageText();
        }

        private string GetValidationMessageText()
        {
            if (CurrentOrder.TargetRequirement == TargetRequirement.None)
                return null;
            if (CurrentOrder.TargetRequirement == TargetRequirement.Point)
                return null;
            if (CurrentOrder.TargetRequirement == TargetRequirement.Unit && ! _selector.CurrentTarget.Unit)
                return "Must target a unit";
            if ( ! _selector.CurrentTarget.Unit)
                return null;
            
            AbilityOrder abilityOrder = CurrentOrder as AbilityOrder;
            if ( ! abilityOrder)
                return null;

            Unit[] actors = Selection.SelectedUnits.Where(u => u.Type.AvailableOrders.Contains(abilityOrder)).ToArray();

            string invalidMessage = "";
            foreach (Unit actor in actors)
            {
                if (abilityOrder.AbilityType.TargetValidators.IsValid(actor, _selector.CurrentTarget.Unit, out invalidMessage))
                    return null;
            }
            return invalidMessage;
        }
    }
}