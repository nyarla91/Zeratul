using Gameplay.Player;
using Zenject;

namespace Architecture
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        { 
            Container.Bind<PlayerInput>().AsSingle();
            Container.Bind<PlayerMouseTargeting>().AsSingle();
            Container.Bind<PlayerSelection>().AsSingle();
            Container.Bind<PlayerOrdersDispatcher>().AsSingle();
            Container.Bind<PlayerOrderTargeter>().AsSingle();
            Container.Bind<PlayerControlResources>().AsSingle();
        }
    }
}