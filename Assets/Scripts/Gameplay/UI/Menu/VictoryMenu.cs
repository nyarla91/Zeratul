using _Core.Pause;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Menu
{
    public class VictoryMenu : MonoBehaviour
    {
        [SerializeField] private UIUtility.Menu _menu;
        
        [Inject] private GamePause GamePause { get; set; }

        public void Open()
        {
            _menu.Open();
            GamePause.Pause(this);
        }
    }
}