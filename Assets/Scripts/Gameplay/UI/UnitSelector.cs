using System;
using System.Linq;
using Extentions;
using Extentions.Pause;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;
using UniRx;
using PlayerInput = Gameplay.Player.PlayerInput;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.UI
{
    
    public class UnitSelector : MonoBehaviour
    {
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private int _beginDragEventIndex;
        [SerializeField] private int _endDragEventIndex;
        [SerializeField] private int _clickEventIndex;
        [SerializeField] private LayerMask _unitsMask;
        
        public Vector2 SelectionStartPosition { get; private set; }
        public bool IsSelecting { get; private set; }
        
        [Inject] private ClickArea ClickArea { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] private PlayerInput Input { get; set; }
        [Inject] private PlayerOrderTargeter Targeter { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        
        private void Awake()
        {
            _eventTrigger.triggers[_beginDragEventIndex].callback.AddListener(StartBoxSelection);
            _eventTrigger.triggers[_endDragEventIndex].callback.AddListener(FinishBoxSelection);
            Targeter.ObserveEveryValueChanged(t => t.IsTargeting)
                .Subscribe(UpdateSubscriptions);
        }

        private void UpdateSubscriptions(bool isTargeting)
        {
            if (isTargeting)
            {
                ClickArea.LeftClicked -= SelectSingleUnit;
            }
            else
            {
                ClickArea.LeftClicked += SelectSingleUnit;
            }
        }

        private void StartBoxSelection(BaseEventData _)
        {
            if (GamePause.IsPaused || IsSelecting || ! Mouse.current.leftButton.isPressed)
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
            
            Unit[] selectedUnits = GetUnitsFromColliders(Physics2D.OverlapBoxAll(overlapBoxOrigin, overlapBoxSize, 0, _unitsMask));
            if (selectedUnits.Length == 0)
                return;
            
            if (Input.SelectMultiple.IsHeld)
                Selection.AddUnitsToSelection(selectedUnits);
            else
                Selection.SelectUnits(selectedUnits);
            
        }

        private static Unit[] GetUnitsFromColliders(Collider2D[] colliders) =>
            colliders.Select(unit => unit.GetComponentInParent<Unit>()).NoNull()
                .Where(unit => unit.Alliance.OwnedByPlayer).NoNull();

        private void SelectSingleUnit()
        {
            if (GamePause.IsPaused || ! Mouse.current.leftButton.wasReleasedThisFrame)
                return;
            if (MouseTargeting.Unit == null)
                return;
            
            if (Input.SelectMultiple.IsHeld)
                Selection.ToggleUnitSelection(MouseTargeting.Unit);
            else
                Selection.SelectUnits(MouseTargeting.Unit);
        }

        private void Update()
        {
            if (GamePause.IsPaused)
                IsSelecting = false;
        }
    }
}