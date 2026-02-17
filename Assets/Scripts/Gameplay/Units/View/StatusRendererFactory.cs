using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Statuses;
using Gameplay.Units.View.StatusRendering;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class StatusRendererFactory
    {
        private ContainerInstantiator _containerInstantiator;
        private StatusRenderer _prefab;
        
        private readonly HashSet<StatusRenderer> _pool =  new();

        public StatusRendererFactory(ContainerInstantiator containerInstantiator, StatusRenderer prefab)
        {
            _containerInstantiator = containerInstantiator;
            _prefab = prefab;
        }

        public StatusRenderer GetStatusRenderer(Status status)
        {
            StatusRenderer result = _pool.FirstOrDefault();
            
            if ( ! result)
            {
                result = _containerInstantiator.Instantiate<StatusRenderer>(_prefab.gameObject, Vector3.zero);
                result.Init(this);
            }
            else
            {
                _pool.Remove(result);
            }
            
            result.OnShow(status);
            result.gameObject.SetActive(true);
            return result;
        }

        public void ReleaseStatusRenderer(StatusRenderer statusRenderer)
        {
            statusRenderer.OnHide();
            statusRenderer.transform.SetParent(null);
            statusRenderer.gameObject.SetActive(false);
            _pool.Add(statusRenderer);
        }
    }
}