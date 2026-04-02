using Gameplay;
using Gameplay.Units.View.StatusRendering;
using Zenject;

namespace DI
{
    public class GameplayFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactory<StatusRenderer>();
        }

        private void BindFactory<TElement>() where TElement : PoolElement<TElement>
        {
            PoolFactory<TElement> factory = new();
            Container.Inject(factory);
            Container.BindInstance(factory).AsSingle();
        }
        
    }
}