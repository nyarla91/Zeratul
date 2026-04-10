namespace GameState.States
{
    public class MainMenuState : IGameState
    {
        private readonly SceneLoader _sceneLoader;

        public MainMenuState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.LoadMainScene();
        }

        public void Exit()
        {
            
        }
    }
}