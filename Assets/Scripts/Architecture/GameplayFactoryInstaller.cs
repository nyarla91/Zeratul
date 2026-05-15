using Gameplay;
using Gameplay.Entities;
using Gameplay.Units.View.StatusRendering;
using Gameplay.Visual;
using UnityEngine;
using Zenject;

namespace Architecture
{
    public class GameplayFactoryInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _aoePrefab;
        
        public override void InstallBindings()
        {
            BindFactory<StatusRenderer>();
            BindFactory<Projectile>();
            BindFactory<Entity>();
            BindFactory<AoeView>(_aoePrefab);
        }

        private void BindFactory<TElement>(GameObject prefab = null) where TElement : PoolElement<TElement>
        {
            PoolFactory<TElement> factory = new(prefab);
            Container.Inject(factory);
            Container.BindInstance(factory).AsSingle();
        }
        
    }
}