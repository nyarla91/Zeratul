using Settings.Localization;
using Zenject;

namespace GameState.States
{
    public class BootstrapState : IGameState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly Localizer _localizer;

        public bool Finished => true;

        [Inject]
        public BootstrapState(SceneLoader sceneLoader, Localizer localizer)
        {
            _sceneLoader = sceneLoader;
            _localizer = localizer;
        }

        public void Enter()
        {
            _localizer.GenerateDictionaries();
        }

        public void Exit()
        {
            
        }
    }
}