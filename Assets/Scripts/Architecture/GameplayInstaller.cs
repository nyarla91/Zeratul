using Extentions.Pause;
using Gameplay;
using Gameplay.Arrangement;
using Gameplay.Arrangement.Saving;
using Gameplay.Data;
using Gameplay.Map;
using Gameplay.UI;
using Gameplay.Units;
using Gameplay.Vision;
using Gameplay.Visual;
using UnityEngine;
using Zenject;

namespace Architecture
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private NodeMap _nodeMap;
        [SerializeField] private IsometricOverlap _isometricOverlap;
        [SerializeField] private VisionMap _visionMap;
        [SerializeField] private Tooltip _tooltip;
        [SerializeField] private RangeEllipseFactory _rangeEllipseFactory;
        [SerializeField] private OrderErrorMessage _orderErrorMessage;
        [SerializeField] private TutorialWindow _tutorialWindow;
        [SerializeField] private ScenarioLifetime _scenarioLifetime;
        [SerializeField] private GameplaySaveLoad _gameplaySaveLoad;
        [SerializeField] private UnitSpawner _unitSpawner;
        [SerializeField] private GameDataRegistry _gameDataRegistry;
        [SerializeField] private ClickArea _clickArea;
        
        public override void InstallBindings()
        {
            Container.Bind<GamePause>().AsSingle();
            Container.Bind<TacticalPause>().AsSingle();
            Container.Bind<GameTime>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnitPool>().AsSingle();
            
            Container.BindInstance(_nodeMap).AsSingle();
            Container.BindInstance(_isometricOverlap).AsSingle();
            Container.BindInstance(_visionMap).AsSingle();
            Container.BindInstance(_tooltip).AsSingle();
            Container.BindInstance(_rangeEllipseFactory).AsSingle();
            Container.BindInstance(_orderErrorMessage).AsSingle();
            Container.BindInstance(_tutorialWindow).AsSingle();
            Container.BindInstance(_scenarioLifetime).AsSingle();
            Container.BindInstance(_gameplaySaveLoad).AsSingle();
            Container.BindInstance(_unitSpawner).AsSingle();
            Container.BindInstance(_gameDataRegistry).AsSingle();
            Container.BindInstance(_clickArea).AsSingle();
        }
    }
}