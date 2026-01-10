using Extentions;
using Extentions.Pause;
using Gameplay;
using Gameplay.Data;
using Gameplay.Pathfinding;
using Gameplay.Player;
using Gameplay.UI;
using Gameplay.Units;
using Gameplay.Vision;
using Gameplay.Visual;
using UnityEngine;

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
        
        [SerializeField] private SOInjectPresenter _injectPresenter;
        
        public override void InstallBindings()
        {
            Pause pause = new();
            Container.Bind<IPauseRead>().FromInstance(pause).AsSingle();
            Container.Bind<IPauseSet>().FromInstance(pause).AsSingle();
            
            BindFromInstance(_playerSelection);
            BindFromInstance(_unitPool);
            BindFromInstance(_playerInput);
            BindFromInstance(_playerOrderTargetSelector);
            BindFromInstance(_playerOrdersDispatcher);
            BindFromInstance(_nodeMap);
            BindFromInstance(_isometricOverlap);
            BindFromInstance(_visionMap);
            BindFromInstance(_tooltip);
            BindFromInstance(_rangeEllipseFactory);

            _injectPresenter.Init(Container);
        }
    }
}