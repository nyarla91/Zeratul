using System;
using Extentions.Input;
using Extentions.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using InputBinding = Extentions.Input.InputBinding;

namespace Gameplay.Player
{
    public class PlayerInput
    {
        private readonly InputActions _actions;

        public InputBinding SelectMultiple { get; }
        public InputBinding QueueOrder { get; }
        public InputBinding DragCamera { get; }
        public InputBinding FocusNextUnitType { get; }
        public InputBinding ToggleTacticalPause { get; }
        public InputBinding QuickSave { get; }
        public InputBinding QuickLoad { get; }
        public InputBinding ToggleEnemyVision { get; }

        public float ZoomDelta => _actions.General.ZoomDelta.ReadValue<float>();

        [Inject]
        public PlayerInput(GamePause pause, ScenarioCompletion scenarioCompletion)
        {
            _actions = new InputActions();
            SelectMultiple = new InputBinding(_actions.General.SelectMultiple, pause);
            QueueOrder = new InputBinding(_actions.General.QueueOrder, pause);
            DragCamera = new InputBinding(_actions.General.DragCamera, pause);
            FocusNextUnitType = new InputBinding(_actions.General.FocusNextUnitType, pause);
            ToggleTacticalPause = new InputBinding(_actions.General.ToggleTacticalPause, pause);
            QuickSave = new InputBinding(_actions.General.QuickSave, pause);
            QuickLoad = new InputBinding(_actions.General.QuickLoad, pause);
            ToggleEnemyVision = new InputBinding(_actions.General.ToggleEnemyVision, pause);
            _actions.Enable();

            scenarioCompletion.Completed += Dispose;
        }

        public InputAction GetOrderHotkeyAction(string alias) => _actions.FindAction(alias);

        public void Dispose()
        {
            SelectMultiple.Dispose();
            QueueOrder.Dispose();
            DragCamera.Dispose();
            FocusNextUnitType.Dispose();
            ToggleTacticalPause.Dispose();
            QuickSave.Dispose();
            QuickLoad.Dispose();
            ToggleEnemyVision.Dispose();
        }
    }
}