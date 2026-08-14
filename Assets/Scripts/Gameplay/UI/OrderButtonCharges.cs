using System.Linq;
using _Core;
using Gameplay.Data.Abilities;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class OrderButtonCharges : MonoBehaviour
    {
        [SerializeField] private OrderButton _button;
        [SerializeField] private TMP_Text _chargesText;
        
        [Inject] private PlayerSelection Selection { get; set; }

        private void Update()
        {
            AbilityOrder abilityOrder = _button.OrderType as AbilityOrder;
            if ( ! abilityOrder || abilityOrder.AbilityType.ChargesToUse < 1)
            {
                _chargesText.text = "";
                return;
            }
            AbilityType abilityType = abilityOrder.AbilityType;
            Ability[] unitAbilities = Selection.SelectedUnits.Select(u => u.Abilities.GetAbility(abilityType)).ClearNull();
            _chargesText.text = unitAbilities.Sum(a => a.Charges).ToString();
        }
    }
}