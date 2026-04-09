using Scenes;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Menu
{
    public class RestartButton : MonoBehaviour
    {
        [Inject] private SceneLoader SceneLoader { get; set; }

        public void Restart()
        {
            SceneLoader.LoadGameplay();
        }
    }
}