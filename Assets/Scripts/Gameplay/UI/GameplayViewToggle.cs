using _Core.Input;
using Gameplay.Player;
using UIUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public abstract class GameplayViewToggle : MonoBehaviour
    {
        [SerializeField] private int _pointerClickEventIndex;
        [SerializeField] private Image _image;
        [SerializeField] private Color _onColor;
        [SerializeField] private Color _offColor;
        [SerializeField] private ClickSound _clickSound;

        [Inject] private PlayerInput PlayerInput { get; set; }

        protected abstract bool IsOn { get; }

        protected virtual void Awake()
        {
            GetInputBinding(PlayerInput).Performed += Toggle;
            UpdateImageColor();
        }

        public void Toggle()
        {
            _clickSound.PlayToggle( ! IsOn);
            if (IsOn)
                ToggleOff();
            else
                ToggleOn();
            UpdateImageColor();
        }

        protected abstract InputBinding GetInputBinding(PlayerInput playerInput);
        
        protected abstract void ToggleOn();
        protected abstract void ToggleOff();

        private void UpdateImageColor()
        {
            _image.color = IsOn ? _onColor : _offColor;
        }

        private void OnPointerClick(BaseEventData _)
        {
            Toggle();
        }
    }
}
