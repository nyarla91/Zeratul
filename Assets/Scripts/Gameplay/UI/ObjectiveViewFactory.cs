using System;
using System.Collections.Generic;
using _Core;
using UnityEngine;

namespace Gameplay.UI
{
    public class ObjectiveViewFactory : MonoBehaviour
    {
        [SerializeField] private GameObject _viewPrefab;

        private List<ObjectiveView> _views = new();

        public void InitView(Func<Objective> objective, int priority)
        {
            ObjectiveView view = Instantiate(_viewPrefab, transform).GetComponent<ObjectiveView>();
            view.Init(objective, priority);

            for (int i = _views.Count - 1; i >= 0; i--)
            {
                if (i > 0 && priority >= _views[i].Priority)
                    continue;
                _views.Insert(i, view);
                break;
            }
            OrderByPriority();
        }

        private void OrderByPriority()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].transform.SetAsLastSibling();
            }
        }
    }
}