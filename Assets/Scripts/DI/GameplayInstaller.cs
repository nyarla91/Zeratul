using Extentions;
using Extentions.Pause;
using Gameplay;
using Gameplay.Pathfinding;
using Gameplay.Player;
using UnityEngine;

namespace DI
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSelection _playerSelection;
        [SerializeField] private PlayerOwnership _playerOwnership;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerOrderTargetSelector _playerOrderTargetSelector;
        [SerializeField] private PlayerOrdersDispatcher _playerOrdersDispatcher;
        [SerializeField] private NodeMap _nodeMap;
        [SerializeField] private IsometricOverlap _isometricOverlap;
        
        public override void InstallBindings()
        {
            Pause pause = new();
            Container.Bind<IPauseRead>().FromInstance(pause).AsSingle();
            Container.Bind<IPauseSet>().FromInstance(pause).AsSingle();
            
            BindFromInstance(_playerSelection);
            BindFromInstance(_playerOwnership);
            BindFromInstance(_playerInput);
            BindFromInstance(_playerOrderTargetSelector);
            BindFromInstance(_playerOrdersDispatcher);
            BindFromInstance(_nodeMap);
            BindFromInstance(_isometricOverlap);
        }
    }
}