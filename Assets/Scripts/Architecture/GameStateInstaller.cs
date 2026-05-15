using System.Collections.Generic;
using GameState;
using GameState.States;
using Zenject;

namespace Architecture
{
    public class GameStateInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            List<IGameState> states = new()
            {
                Container.Instantiate<BootstrapState>(),
                Container.Instantiate<MainMenuState>(),
                Container.Instantiate<GameplayState>(),
            };

            Container.Bind<GameStateMachine>().FromInstance(new GameStateMachine(states)).AsSingle().NonLazy();
            Container.Bind<GameFlowController>().AsSingle().NonLazy();
        }
    }
}