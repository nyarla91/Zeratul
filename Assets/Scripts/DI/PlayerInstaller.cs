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
        [SerializeField] private PlayerControlConfig _controlConfig;
        [SerializeField] private LayerMask _unitsMask;
        
        private PlayerInput _input;
        private PlayerMouseTargeting _mouseTargeting;
        private PlayerOrdersDispatcher _ordersDispatcher;
        private PlayerOrderTargeter _orderTargeter;
        private PlayerSelection _selection;
        private PlayerControlResources _controlResources;
        
        public override void InstallBindings()
        {
            _input =  new PlayerInput();
            _mouseTargeting = new PlayerMouseTargeting(Camera.main, _unitsMask);
            _selection = new PlayerSelection(_input);
            _ordersDispatcher = new PlayerOrdersDispatcher(_selection, _input, _errors);
            _orderTargeter = new PlayerOrderTargeter(_mouseTargeting, _selection, _ordersDispatcher);
            _controlResources = new PlayerControlResources(_controlConfig);

            Container.Inject(_input);
            Container.Inject(_mouseTargeting);
            Container.Inject(_selection);
            Container.Inject(_ordersDispatcher);
            Container.Inject(_orderTargeter);
            Container.Inject(_controlResources);
            
            Container.Bind<PlayerInput>().FromInstance(_input).AsSingle();
            Container.Bind<PlayerMouseTargeting>().FromInstance(_mouseTargeting).AsSingle();
            Container.Bind<PlayerSelection>().FromInstance(_selection).AsSingle();
            Container.Bind<PlayerOrdersDispatcher>().FromInstance(_ordersDispatcher).AsSingle();
            Container.Bind<PlayerOrderTargeter>().FromInstance(_orderTargeter).AsSingle();
            Container.Bind<PlayerControlResources>().FromInstance(_controlResources).AsSingle();
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }
    }
}