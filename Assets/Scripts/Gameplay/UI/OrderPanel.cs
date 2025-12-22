using Gameplay.Data;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class OrderPanel : MonoBehaviour
    {
        [SerializeField] private OrderButton[] _buttons;
        
        [Inject] private PlayerSelection PlayerSelection { get; set; }

        private void Awake()
        {
            PlayerSelection.SelectionUpdated += UpdateButtons;
        }

        private void UpdateButtons(Unit[] selection)
        {
            UnitType unitType = selection.Length == 0 ? null : selection[0]?.Type;
            bool clear = unitType == null;
            
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (clear)
                {
                    _buttons[i].ApplyOrderType(null);
                    continue;
                }
                OrderType orderType = (i < unitType.AvailableOrders.Length) ?  unitType.AvailableOrders[i] : null;
                _buttons[i].ApplyOrderType(orderType);
            }
        }
    }
}