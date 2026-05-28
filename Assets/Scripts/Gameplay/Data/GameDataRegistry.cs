using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Game Data Registry", order = 0)]
    public class GameDataRegistry : ScriptableObject
    {
        [SerializeField] private Object[] _objects;

        private Dictionary<string, Object> _registry;

        public T Get<T>(string name) where T : Object
        {
            _registry ??= _objects.ToDictionary(o => o.name, o => o);

            if ( ! _registry.TryGetValue(name, out Object result))
                throw new KeyNotFoundException($"{this.name} does not contain object {name} of type {typeof(T).Name}");
            return (T) result;
        }

        private void OnValidate()
        {
            _objects = _objects.ToHashSet().ToArray();
        }
    }
}