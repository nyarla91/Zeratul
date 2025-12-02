using System.Linq;
using Gameplay.Player;
using Gameplay.Units;
using Source.Extentions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;
using PlayerInput = Gameplay.Player.PlayerInput;

namespace Gameplay.UnitManagement
{
    
    public class UnitSelectior : MonoBehaviour
    {
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _beginDragEventIndex;
        [SerializeField] private int _endDragEventIndex;
        [SerializeField] private int _clickEventIndex;
        
        public Vector2 SelectionStartPosition { get; private set; }
        public bool IsSelecting { get; private set; }
        
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        
        [Inject] private PlayerInput PlayerInput { get; set; }
        
        private void Awake()
        {
            _eventTrigger.triggers[_beginDragEventIndex].callback.AddListener(StartBoxSelection);
            _eventTrigger.triggers[_endDragEventIndex].callback.AddListener(FinishBoxSelection);
            _eventTrigger.triggers[_clickEventIndex].callback.AddListener(SelectSingleUnit);
        }

        private void StartBoxSelection(BaseEventData _)
        {
            if (IsSelecting || ! Mouse.current.leftButton.isPressed)
                return;
            SelectionStartPosition = Mouse.current.position.ReadValue();
            IsSelecting = true;
        }

        private void FinishBoxSelection(BaseEventData _)
        {
            if ( ! IsSelecting || ! Mouse.current.leftButton.wasReleasedThisFrame)
                return;
            
            IsSelecting = false;
            
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPointA = Camera.main.ScreenToWorldPoint(SelectionStartPosition);
            Vector2 worldPointB = Camera.main.ScreenToWorldPoint(currentMousePosition);
            
            Vector2 overlapBoxOrigin = Vector2.Lerp(worldPointA, worldPointB, 0.5f);
            Vector2 overlapBoxSize = (worldPointB - worldPointA).Abs();
            
            Unit[] selectedUnits = GetUnitsFromColliders(Physics2D.OverlapBoxAll(overlapBoxOrigin, overlapBoxSize, 0));
            if (selectedUnits.Length == 0)
                return;
            
            if (PlayerInput.SelectMultipleBinding.IsHeld)
                PlayerSelection.AddUnitsToSelection(selectedUnits);
            else
                PlayerSelection.SelectUnits(selectedUnits);
            
        }

        private static Unit[] GetUnitsFromColliders(Collider2D[] colliders) =>
            colliders.Select(unit => unit.GetComponent<Unit>()).ClearNull()
                .Where(unit => unit.Ownership.OwnedByPlayer).ClearNull();

        private void SelectSingleUnit(BaseEventData arg)
        {
            if (! Mouse.current.leftButton.wasReleasedThisFrame)
                return;
            
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(currentMousePosition);
            
            Unit[] selectedUnits = GetUnitsFromColliders(Physics2D.OverlapPointAll(worldPoint));
            if (selectedUnits.Length == 0)
                return;
            
            if (PlayerInput.SelectMultipleBinding.IsHeld)
                PlayerSelection.ToggleUnitSelection(selectedUnits[0]);
            else
                PlayerSelection.SelectUnits(selectedUnits);
        }
        
    }
}