using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Game Data Registry", order = 0)]
    public class GameDataRegistry : ScriptableObject
    {
        [SerializeField] private ScriptableObject[] _objects;

        private Dictionary<string, ScriptableObject> _registry;

        private void GenerateRegistry()
        {
            _registry = _objects.ToDictionary(o => o.name, o => o);
        }

        public T Get<T>(string name) where T : ScriptableObject
        {
            if (_registry == null)
                GenerateRegistry();

            if ( ! _registry.TryGetValue(name, out ScriptableObject result))
            {
                throw new KeyNotFoundException($"{this.name} does not contain object {name} of type {typeof(T).Name}");
            }
            return (T) result;
        }
    }
}