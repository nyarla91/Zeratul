using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Statuses;
using Gameplay.Units.View.StatusRendering;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class StatusRendererFactory
    {
        private readonly Dictionary<GameObject, List<StatusRenderer>> _pool = new();
        
        [Inject] private ContainerInstantiator ContainerInstantiator { get; set; }

        public StatusRenderer Get(GameObject prefab)
        {
            if ( ! _pool.TryGetValue(prefab, out List<StatusRenderer> pool))
            {
                return Instantiate(prefab);
            }
            StatusRenderer result = pool.FirstOrDefault(sr => ! sr.gameObject.activeSelf) ?? Instantiate(prefab);
            result.gameObject.SetActive(true);
            return result;
        }

        private StatusRenderer Instantiate(GameObject prefab)
        {
            if ( ! _pool.TryGetValue(prefab, out List<StatusRenderer> pool))
            {
                pool = new List<StatusRenderer>();
                _pool.Add(prefab, pool);
            }
            StatusRenderer result = ContainerInstantiator.Instantiate<StatusRenderer>(prefab.gameObject, Vector3.zero);
            result.Init(this);
            pool.Add(result);
            return result;
        }

        public void ReleaseStatusRenderer(StatusRenderer statusRenderer)
        {
            statusRenderer.OnRemove();
            statusRenderer.transform.SetParent(null);
            statusRenderer.gameObject.SetActive(false);
        }
    }
}