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
    public class GameplayMonoInstaller : MonoInstaller
    {
        [SerializeField] private NodeMap _nodeMap;
        [SerializeField] private IsometricOverlap _isometricOverlap;
        [SerializeField] private VisionMap _visionMap;
        [SerializeField] private Tooltip _tooltip;
        [SerializeField] private RangeEllipseFactory _rangeEllipseFactory;
        [SerializeField] private Message _message;
        [SerializeField] private TutorialWindow _tutorialWindow;
        [SerializeField] private ScenarioLifetime _scenarioLifetime;
        [SerializeField] private GameplaySaveLoad _gameplaySaveLoad;
        [SerializeField] private UnitSpawner _unitSpawner;
        [SerializeField] private GameDataRegistry _gameDataRegistry;
        [SerializeField] private ClickArea _clickArea;
        [SerializeField] private TacticalPauseControl _tacticalPauseControl;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_nodeMap).AsSingle();
            Container.BindInstance(_isometricOverlap).AsSingle();
            Container.BindInstance(_visionMap).AsSingle();
            Container.BindInstance(_tooltip).AsSingle();
            Container.BindInstance(_rangeEllipseFactory).AsSingle();
            Container.BindInstance(_message).AsSingle();
            Container.BindInstance(_tutorialWindow).AsSingle();
            Container.BindInstance(_scenarioLifetime).AsSingle();
            Container.BindInstance(_gameplaySaveLoad).AsSingle();
            Container.BindInstance(_unitSpawner).AsSingle();
            Container.BindInstance(_gameDataRegistry).AsSingle();
            Container.BindInstance(_clickArea).AsSingle();
            Container.BindInstance(_tacticalPauseControl).AsSingle();
        }
    }
}