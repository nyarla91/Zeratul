using System;
using Gameplay.Data.Configs;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace DI
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private OrderErrorConfig _errors;
        [SerializeField] private LayerMask _unitsMask;
        
        private PlayerInput _input;
        private PlayerOrdersDispatcher _ordersDispatcher;
        private PlayerOrderTargetSelector _orderTargetSelector;
        private PlayerSelection _selection;
        
        public override void InstallBindings()
        {
            _input =  new PlayerInput();
            _selection = new PlayerSelection(_input);
            _ordersDispatcher = new PlayerOrdersDispatcher(_selection, _input, _errors);
            _orderTargetSelector = new PlayerOrderTargetSelector(_unitsMask);

            Container.Inject(_input);
            Container.Inject(_selection);
            Container.Inject(_ordersDispatcher);
            Container.Inject(_orderTargetSelector);
            
            Container.Bind<PlayerInput>().FromInstance(_input).AsSingle();
            Container.Bind<PlayerSelection>().FromInstance(_selection).AsSingle();
            Container.Bind<PlayerOrdersDispatcher>().FromInstance(_ordersDispatcher).AsSingle();
            Container.Bind<PlayerOrderTargetSelector>().FromInstance(_orderTargetSelector).AsSingle();
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }
    }
}