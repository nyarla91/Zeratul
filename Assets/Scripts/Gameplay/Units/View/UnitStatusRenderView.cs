using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.Statuses;
using Gameplay.Units.View.StatusRendering;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitStatusRenderView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private Dictionary<StatusType, StatusRenderer[]> _renderers = new();
        
        [Inject] private PoolFactory<StatusRenderer> StatusRendererFactory { get; set; }

        private void Start()
        {
            _unit.Statuses.StatusAdded += AddRenderers;
            _unit.Statuses.StatusRemoved += RemoveRenderers;
            _unit.Killed += HideAllRenderers;

            foreach (IStatusInfo status in _unit.Statuses.StatusesInfo)
            {
                AddRenderers(status);
            }
        }
        
        private void AddRenderers(IStatusInfo status)
        {
            StatusRenderer[] renderers =  status.Type.RendererPrefabs.Select(r => StatusRendererFactory.Get(r.gameObject)).ToArray();
            foreach (StatusRenderer statusRenderer in renderers)
            {
                statusRenderer.transform.SetParent(transform);
                statusRenderer.transform.localPosition = Vector3.zero;
                statusRenderer.Status = status;
            }
            _renderers.Add(status.Type, renderers);
        }

        private void RemoveRenderers(IStatusInfo status) => RemoveRenderers(status.Type);
        
        private void RemoveRenderers(StatusType status)
        {
            if ( ! _renderers.TryGetValue(status, out StatusRenderer[] renderers))
                return;

            foreach (StatusRenderer renderer in renderers)
                renderer.Despawn();
            
            _renderers.Remove(status);
        }

        private void HideAllRenderers()
        {
            foreach (StatusType status in _renderers.Keys.ToList())
            {
                RemoveRenderers(status);
            }
        }
    }
}