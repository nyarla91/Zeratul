using Extentions.Pause;
using Gameplay;
using Gameplay.Data;
using Gameplay.Pathfinding;
using Gameplay.Player;
using Gameplay.UI;
using Gameplay.Units;
using Gameplay.Units.View;
using Gameplay.Vision;
using Gameplay.Visual;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSelection _playerSelection;
        [SerializeField] private UnitPool _unitPool;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerOrderTargetSelector _playerOrderTargetSelector;
        [SerializeField] private PlayerOrdersDispatcher _playerOrdersDispatcher;
        [SerializeField] private NodeMap _nodeMap;
        [SerializeField] private IsometricOverlap _isometricOverlap;
        [SerializeField] private VisionMap _visionMap;
        [SerializeField] private Tooltip _tooltip;
        [SerializeField] private RangeEllipseFactory _rangeEllipseFactory;
        [SerializeField] private StatusRendererFactoryFactory _statusRendererFf;
        [SerializeField] private OrderErrorMessage _orderErrorMessage;
        
        [SerializeField] private SOInjectPresenter _injectPresenter;
        
        public override void InstallBindings()
        {
            Container.Bind<GamePause>().AsSingle();
            Container.Bind<TacticalPause>().AsSingle();
            
            Container.BindInstance(_playerSelection).AsSingle();
            Container.BindInstance(_unitPool).AsSingle();
            Container.BindInstance(_playerInput).AsSingle();
            Container.BindInstance(_playerOrderTargetSelector).AsSingle();
            Container.BindInstance(_playerOrdersDispatcher).AsSingle();
            Container.BindInstance(_nodeMap).AsSingle();
            Container.BindInstance(_isometricOverlap).AsSingle();
            Container.BindInstance(_visionMap).AsSingle();
            Container.BindInstance(_tooltip).AsSingle();
            Container.BindInstance(_rangeEllipseFactory).AsSingle();
            Container.BindInstance(_statusRendererFf).AsSingle();
            Container.BindInstance(_orderErrorMessage).AsSingle();

            _injectPresenter.Init(Container);
        }
    }
}