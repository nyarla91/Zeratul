using System.Collections.Generic;
using Extentions;
using Gameplay.Data.Statuses;
using Gameplay.Units.View.StatusRendering;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class StatusRendererFactoryFactory : MonoBehaviour
    {
        private readonly Dictionary<StatusType, StatusRendererFactory[]> _factories = new();

        [Inject] private ContainerInstantiator ContainerInstantiator { get; set; }
        
        public StatusRendererFactory[] GetFactoriesForStatus(StatusType status)
        {
            if (_factories.TryGetValue(status, out StatusRendererFactory[] result))
                return result;
            
            result = new StatusRendererFactory[status.RendererPrefabs.Length];

            for (int i = 0; i < result.Length; i++)
            {
                StatusRenderer prefab = status.RendererPrefabs[i];
                StatusRendererFactory instance = new(ContainerInstantiator, prefab);
                result[i] = instance;
            }
            
            _factories.Add(status, result);
            return result;
        }
    }
}