using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Scriptable Object Inject Presenter", order = 0)]
    public class SOInjectPresenter : ScriptableObject
    {
        private readonly List<ScriptableObject> _injected = new();
        private DiContainer _container;

        public void Init(DiContainer container)
        {
            if (_container != null)
                return;
            _container = container;
        }
        
        public void Inject(ScriptableObject so)
        {
            if (_injected.Contains(so))
                return;
            _container.Inject(so);
            _injected.Add(so);
        }

        public void Dispose()
        {
            _container = null;
            _injected.Clear();
        }
    }
}