using System.Linq;
using _Core.Pause;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Cheats
{
    public class CheatMenu : MonoBehaviour
    {
        public static bool IsInvulnerableToggled { get; private set; }
        public static bool IsUndetectedToggled { get; private set; }
        public static bool IsSpeedBoostToggled { get; private set; }
        public static bool IsFreeAbilitiesToggled { get; private set; }
        public static bool IsEagleVisionToggled { get; private set; }
        
        [SerializeField] private KeyCode _toggleKey = KeyCode.Backslash;
        [SerializeField] private EventModifiers _toggleModifiers = EventModifiers.Control | EventModifiers.Shift;
        [SerializeField] private Rect _windowRect = new Rect(16f, 16f, 320f, 400f);

        private Vector2 _optionsScrollPosition;
        private bool _isOpen;
        private Unit _playerUnit;

        [Inject] private GamePause GamePause { get; set; }
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }

        private void OnGUI()
        {
            HandleToggleShortcut();

            if (!_isOpen)
                return;

            _windowRect = GUI.Window(263256, _windowRect, DrawWindow, "Debug Menu");
        }

        private void OnDisable() => Close();

        private void HandleToggleShortcut()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type != EventType.KeyDown)
                return;
            if (currentEvent.keyCode != _toggleKey)
                return;
            if (!IsModifiersPressed(currentEvent))
                return;

            currentEvent.Use();

            if (_isOpen)
                Close();
            else
                Open();
        }

        private bool IsModifiersPressed(Event currentEvent)
        {
            if (currentEvent.control != _toggleModifiers.HasFlag(EventModifiers.Control))
                return false;
            if (currentEvent.shift != _toggleModifiers.HasFlag(EventModifiers.Shift))
                return false;
            if (currentEvent.alt != _toggleModifiers.HasFlag(EventModifiers.Alt))
                return false;

            return true;
        }

        private void DrawWindow(int windowId)
        {
            _optionsScrollPosition = GUILayout.BeginScrollView(_optionsScrollPosition, GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Add 1 Control Reserve"))
                PlayerControlResources.AddReserve(1);
            if (GUILayout.Button("Add 5 Control Reserve"))
                PlayerControlResources.AddReserve(5);
            if (GUILayout.Button("Add 1 of each consumable"))
                AddConsumables(1);
            if (GUILayout.Button("Add 5 of each consumable"))
                AddConsumables(5);

            IsInvulnerableToggled = GUILayout.Toggle(IsInvulnerableToggled, "Invulnerable");
            IsUndetectedToggled = GUILayout.Toggle(IsUndetectedToggled, "Undetectable");
            IsSpeedBoostToggled = GUILayout.Toggle(IsSpeedBoostToggled, "Speed boost");
            IsFreeAbilitiesToggled = GUILayout.Toggle(IsFreeAbilitiesToggled, "Free abilities");
            IsEagleVisionToggled = GUILayout.Toggle(IsEagleVisionToggled, "Eagle vision");
                
            GUILayout.EndScrollView();

            if (GUILayout.Button("Close"))
                Close();

            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
        }

        private void AddConsumables(int count)
        {
            foreach (Ability ability in UnitPool.PlayerUnits.SelectMany(unit => unit.Abilities.Abilities.Where(ability => ability.Type.ChargesToUse != 0)))
            {
                ability.AddCharges(count);
            }
        }

        private void Open()
        {
            if (_isOpen)
                return;

            _isOpen = true;
            GamePause?.Pause(this);
        }

        private void Close()
        {
            if (!_isOpen)
                return;

            _isOpen = false;
            GamePause?.Unpause(this);
        }
    }
}
