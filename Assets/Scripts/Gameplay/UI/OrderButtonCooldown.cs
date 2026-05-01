using System;
using System.Linq;
using Extentions;
using Gameplay.Data.Abilities;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class OrderButtonCooldown : MonoBehaviour
    {
        [SerializeField] private OrderButton _button;
        [SerializeField] private CanvasGroup _overlayGroup;
        [SerializeField] private Image _overlayFill;

        [Inject] private PlayerSelection Selection { get; set; }

        private void Update()
        {
            AbilityOrder abilityOrder = _button.OrderType as AbilityOrder;
            if ( ! abilityOrder  || abilityOrder.AbilityType.Cooldown == 0)
            {
                SetFill(0);
                return;
            }
            AbilityType abilityType = abilityOrder.AbilityType;
            Ability[] unitAbilities = Selection.SelectedUnits.Select(u => u.Abilities.GetAbility(abilityType)).NoNull();
            if (unitAbilities.Length == 0 || unitAbilities.Any(a => a.IsReady))
            {
                SetFill(0);
                return;
            }
            float cooldownLeft = unitAbilities.Min(a => a.CooldownLeft);
            float percent = cooldownLeft / abilityType.Cooldown;
            SetFill(percent);
        }

        private void SetFill(float percent)
        {
            if (percent <= 0)
            {
                _overlayGroup.alpha = 0;
                return;
            }
            _overlayGroup.alpha = 1;
            _overlayFill.fillAmount = percent;
        }
    }
}