using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Gameplay.UnitManagement
{
    public class UnitOrderHandler : MonoBehaviour
    {
        [SerializeField] private PlayerSelection _playerSelection;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _clickEventIndex;

        private void Awake()
        {
            _eventTrigger.triggers[_clickEventIndex].callback.AddListener(TryIssueOrder);
        }

        private void TryIssueOrder(BaseEventData arg)
        {
            if ( ! Mouse.current.rightButton.wasReleasedThisFrame)
                return;
            /*Vector2 point =  Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            foreach (Unit selectedUnit in _playerSelection.SelectedUnits)
            {
                selectedUnit.Orders.IssueSmartOrder(point, false);
            }*/
        }
        
    }
}