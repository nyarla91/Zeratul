using Scenes;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        [Inject] private SceneLoader SceneLoader { get; set; }

        public void Play()
        {
            SceneLoader.LoadGameplay(); 
        }
    }
}