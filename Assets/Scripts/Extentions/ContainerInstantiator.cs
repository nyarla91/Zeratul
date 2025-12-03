using UnityEngine;
using Component = UnityEngine.Component;

namespace Extentions
{
    public class ContainerInstantiator : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ContainerInstantiator>().FromInstance(this).AsSingle();
        }

        public T Instantiate<T>(GameObject prefab, Vector3 position, Transform parent = null) where T : Component
            => Container.InstantiatePrefab(prefab, position, Quaternion.identity, parent).GetComponent<T>();
    }
}