using UnityEngine;

namespace Extentions
{
    public class MonoInstaller : Zenject.MonoInstaller
    {
        protected GameObject BindFromInstance<T>(T instance) where T : MonoBehaviour
        {
            Container.Bind<T>().FromInstance(instance).AsSingle();
            return instance.gameObject;
        }
        
        protected GameObject BindFromPrefab<T>(GameObject prefab) where T : MonoBehaviour
        {
            GameObject instance = Container.InstantiatePrefab(prefab, transform);
            BindFromInstance<T>(instance.GetComponent<T>());
            return instance;
        }
    }
}