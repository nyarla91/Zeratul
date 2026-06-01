using System;
using Gameplay.Player;
using Zenject;

namespace Architecture
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        { 
            Container.Bind<PlayerInput>().AsSingle().NonLazy();
            Container.Bind<PlayerMouseTargeting>().AsSingle().NonLazy();
            Container.Bind<PlayerSelection>().AsSingle().NonLazy();
            Container.Bind<PlayerOrdersDispatcher>().AsSingle().NonLazy();
            Container.Bind<PlayerOrderTargeter>().AsSingle().NonLazy();
            Container.Bind<PlayerControlResources>().AsSingle().NonLazy();
            Container.Bind<PlayerUnitRow>().AsSingle().NonLazy();
        }

        private void OnDestroy()
        {
            Container.Resolve<PlayerInput>().Dispose();
            Container.Resolve<PlayerUnitRow>().Dispose();
        }
    }
}