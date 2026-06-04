using _Core.Pause;
using Gameplay;
using Gameplay.Units;
using Zenject;

namespace Architecture
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GamePause>().AsSingle();
            Container.Bind<TacticalPause>().AsSingle();
            Container.Bind<GameTime>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitPool>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScenarioCompletion>().AsSingle();
        }
    }
}