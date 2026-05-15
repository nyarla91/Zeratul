using Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Architecture
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