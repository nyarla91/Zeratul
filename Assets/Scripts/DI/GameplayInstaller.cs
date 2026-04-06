using Extentions.Pause;
using Gameplay;
using Gameplay.Data;
using Gameplay.Map;
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
        [SerializeField] private UnitPool _unitPool;
        [SerializeField] private NodeMap _nodeMap;
        [SerializeField] private IsometricOverlap _isometricOverlap;
        [SerializeField] private VisionMap _visionMap;
        [SerializeField] private Tooltip _tooltip;
        [SerializeField] private RangeEllipseFactory _rangeEllipseFactory;
        [SerializeField] private OrderErrorMessage _orderErrorMessage;
        
        public override void InstallBindings()
        {
            Container.Bind<GamePause>().AsSingle();
            Container.Bind<TacticalPause>().AsSingle();
            
            Container.BindInstance(_unitPool).AsSingle();
            Container.BindInstance(_nodeMap).AsSingle();
            Container.BindInstance(_isometricOverlap).AsSingle();
            Container.BindInstance(_visionMap).AsSingle();
            Container.BindInstance(_tooltip).AsSingle();
            Container.BindInstance(_rangeEllipseFactory).AsSingle();
            Container.BindInstance(_orderErrorMessage).AsSingle();
            
        }
    }
}