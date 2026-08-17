using _Core.Pause;
using Gameplay.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Menu
{
    public class TutorialScreen : TutorialElement
    {
        [SerializeField] private UIUtility.Menu _menu;
        
        [Inject] private GamePause GamePause { get; set; }

        public void Open(TutorialEntry entry)
        {
            Set(entry);
            _menu.Opened += () => GamePause.Pause(this);
            _menu.Closed += () => GamePause.Unpause(this);
            _menu.Open();
        }
    }
}