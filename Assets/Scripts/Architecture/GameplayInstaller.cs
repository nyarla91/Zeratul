using Gameplay;
using Gameplay.Pathfinding;
using Gameplay.Player;
using Source.Extentions;
using Source.Extentions.Pause;
using UnityEngine;

namespace Architecture
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSelection _playerSelection;
        [SerializeField] private PlayerOwnership _playerOwnership;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private NodeMap _nodeMap;
        
        public override void InstallBindings()
        {
            Pause pause = new();
            Container.Bind<IPauseRead>().FromInstance(pause).AsSingle();
            Container.Bind<IPauseSet>().FromInstance(pause).AsSingle();
            
            BindFromInstance(_playerSelection);
            BindFromInstance(_playerOwnership);
            BindFromInstance(_playerInput);
            BindFromInstance(_nodeMap);
        }
    }
}