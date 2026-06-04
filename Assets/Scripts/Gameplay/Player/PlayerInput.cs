using Extentions.Pause;
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
        public InputBinding SelectAllUnits { get; }
        public InputBinding[] SelectUnit { get; }

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
            QuickLoad = new InputBinding(_actions.General.QuickLoad);
            ToggleEnemyVision = new InputBinding(_actions.General.ToggleEnemyVision, pause);
            SelectAllUnits = new InputBinding(_actions.General.SelectAllUnits, pause);
            SelectUnit = new[]
            {
                new InputBinding(_actions.General.SelectUnit1, pause),
                new InputBinding(_actions.General.SelectUnit2, pause),
                new InputBinding(_actions.General.SelectUnit3, pause),
                new InputBinding(_actions.General.SelectUnit4, pause),
                new InputBinding(_actions.General.SelectUnit5, pause),
                new InputBinding(_actions.General.SelectUnit6, pause),
                new InputBinding(_actions.General.SelectUnit7, pause),
                new InputBinding(_actions.General.SelectUnit8, pause),
                new InputBinding(_actions.General.SelectUnit9, pause),
                new InputBinding(_actions.General.SelectUnit10, pause),
            };
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
            foreach (InputBinding inputBinding in SelectUnit)
            {
                inputBinding.Dispose();
            }
        }
    }
}