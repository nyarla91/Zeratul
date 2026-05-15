using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class PoolFactory<TElement> where TElement : PoolElement<TElement>
    {
        private readonly GameObject _defaultPrefab;
        private readonly Dictionary<GameObject, List<TElement>> _pool = new();

        public HashSet<TElement> Pool => _pool.Values.SelectMany(l => l).ToHashSet();
        
        [Inject] private ContainerInstantiator ContainerInstantiator { get; set; }

        public PoolFactory(GameObject defaultPrefab)
        {
            _defaultPrefab = defaultPrefab;
        }
        
        public TElement Get(GameObject prefab = null)
        {
            if ( ! prefab)
            {
                if (_defaultPrefab)
                    prefab = _defaultPrefab;
                else
                    throw new ArgumentNullException($"{this} does not have default prefab");
            }
            
            if ( ! _pool.TryGetValue(prefab, out List<TElement> pool))
            {
                return Instantiate(prefab);
            }
            TElement result = pool.FirstOrDefault(sr => ! sr.gameObject.activeSelf) ?? Instantiate(prefab);
            result.gameObject.SetActive(true);
            result.OnSpawn();
            return result;
        }

        private TElement Instantiate(GameObject prefab)
        {
            if ( ! _pool.TryGetValue(prefab, out List<TElement> pool))
            {
                pool = new List<TElement>();
                _pool.Add(prefab, pool);
            }
            TElement result = ContainerInstantiator.Instantiate<TElement>(prefab.gameObject, Vector3.zero);
            result.Init(this, prefab);
            pool.Add(result);
            return result;
        }

        public void Despawn(PoolElement<TElement> statusRenderer)
        {
            statusRenderer.transform.SetParent(null);
            statusRenderer.gameObject.SetActive(false);
        }
    }
}