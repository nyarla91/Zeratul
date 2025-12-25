using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Data.Statuses;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class StatusPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _viewPrefab;
        [SerializeField] private UnitInfoPanel _unitInfoPanel;
        
        private readonly List<StatusView> _views = new();
        
        [Inject] private ContainerInstantiator ContainerInstantiator { get; set; }

        private void Update()
        {
            if ( ! _unitInfoPanel.CurrentUnit)
                return;
            
            IStatusInfo[] statusesToDisplay = _unitInfoPanel.CurrentUnit.Statuses.StatusesInfo.Where(s => s.Type.Display).ToArray();
            
            AddViews(statusesToDisplay.Length - _views.Count);

            if (statusesToDisplay.Length > _views.Count)
                throw new IndexOutOfRangeException();
            
            for (int i = 0; i < _views.Count; i++)
            {
                bool show = i < statusesToDisplay.Length;
                _views[i].UpdateView(show ? statusesToDisplay[i] : null);
            }
        }

        private void AddViews(int count)
        {
            for (int i = 0; i < count; i++)
            {
                StatusView newView = ContainerInstantiator.Instantiate<StatusView>(_viewPrefab, default, transform);
                _views.Add(newView);
            }
        }
    }
}