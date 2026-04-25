using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Abilities;
using Gameplay.Data.Statuses;
using Gameplay.Data.Units;
using UnityEngine;
using OrderType = Gameplay.Data.Orders.OrderType;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Game Data Registry", order = 0)]
    public class GameDataRegistry : ScriptableObject
    {
        [SerializeField] private UnitType[] _unitTypes;
        [SerializeField] private StatusType[] _statusTypes;
        [SerializeField] private AbilityType[] _abilityTypes;
        [SerializeField] private OrderType[] _orderTypes;

        private Dictionary<Type, GameDataCategory> _categories;

        public void Init()
        {
            AddCategory(_unitTypes);
            AddCategory(_statusTypes);
            AddCategory(_abilityTypes);
            AddCategory(_orderTypes);
        }

        public T Get<T>(string name) where T : ScriptableObject
        {
            if ( ! _categories.TryGetValue(typeof(T), out GameDataCategory category))
                throw new KeyNotFoundException("Category not found: " + typeof(T));
            return category.Get<T>(name);
        }

        private void AddCategory<T>(T[] elements) where T : ScriptableObject
        {
            GameDataCategory category = new(typeof(T), elements);
            _categories.Add(typeof(T), category);
        }

        private class GameDataCategory
        {
            private Type _type;
            private readonly Dictionary<string, ScriptableObject> _elements;

            public T Get<T>(string name) where T : ScriptableObject
            {
                if (typeof(T) != _type)
                    throw new ArgumentException($"The type {_type} does not match the type {typeof(T)}");
                if (_elements.TryGetValue(name, out ScriptableObject element))
                    return element as T;
                throw new KeyNotFoundException($"No element of {typeof(T)} with name {name} found in GameDataRegistry");
            }
            
            public GameDataCategory(Type type, ScriptableObject[] elements)
            {
                _type = type;
                _elements = elements.ToDictionary(e => e.name);
            }
        }
    }
}