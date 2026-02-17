using System;
using System.Collections.Generic;
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
        
        [Inject] private StatusRendererFactoryFactory StatusRendererFf { get; set; }

        private void Awake()
        {
            _unit.Statuses.StatusAdded += ShowRenderer;
            _unit.Statuses.StatusRemoved += HideRenderer;
        }

        private void ShowRenderer(Status status)
        {
            StatusRendererFactory[] factories = StatusRendererFf.GetFactoriesForStatus(status.Type);
            StatusRenderer[] renderers =  new StatusRenderer[factories.Length];
            for (int i = 0; i < factories.Length; i++)
            {
                StatusRenderer newRenderer = factories[i].GetStatusRenderer(status);
                newRenderer.transform.SetParent(transform);
                newRenderer.transform.localPosition = Vector3.zero;
                renderers[i] = newRenderer;
            }
            _renderers.Add(status.Type, renderers);
        }

        private void HideRenderer(Status status)
        {
            if ( ! _renderers.TryGetValue(status.Type, out StatusRenderer[] renderers))
                return;

            foreach (StatusRenderer renderer in renderers) renderer.Release();
            
            _renderers.Remove(status.Type);
        }
    }
}