namespace GameState.States
{
    public class BootstrapState : IGameState
    {
        private readonly SceneLoader _sceneLoader;

        public bool Finished => true;

        public BootstrapState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}