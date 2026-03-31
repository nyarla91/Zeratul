using System;
using Gameplay.Data;
using UnityEngine;
using Zenject;

namespace DI
{
    public class SOInjectPresenterInstaller : MonoInstaller
    {
        [SerializeField] private SOInjectPresenter _injectPresenter;
        
        public override void InstallBindings()
        {
            _injectPresenter.Init(Container);
        }

        private void OnDestroy()
        {
            _injectPresenter.Dispose();
        }
    }
}