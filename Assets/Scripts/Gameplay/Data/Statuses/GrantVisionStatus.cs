using System.Collections.Generic;
using _Core;
using Gameplay.Units;
using Gameplay.Vision;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Grant Vision", order = 0)]
    public class GrantVisionStatus : StatusType
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private float _radius;
        [SerializeField] private bool _useHostRadius;

        private readonly Dictionary<int, VisionSource> _visionSources = new();

        [Inject] private VisionMap VisionMap { get; set; }

        public override void OnAdd(Status status)
        {
            _gameplayPresenter.Inject(this);

            Unit host = status.Host;
            Owner owner = status.Instigator.Alliance.CurrentOwner;
            
            VisionSource source = VisionMap.CreateSource(
                () => host.IsAlive ? host.Position : default,
                () => owner,
                () => _useHostRadius ? (host.IsAlive ? host.Sight.Radius : 0) : _radius,
                () => host.IsAlive && host.Type.IsAir
            );
            _visionSources.Add(host.Id, source);
            host.KilledPayload += DisposeHost;
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            DisposeHost(status.Host);
        }

        private void DisposeHost(Unit host)
        {
            Debug.Log(host.Id);
            host.KilledPayload -= DisposeHost;
            int id = host.Id;
            if ( ! _visionSources.TryGetValue(id, out VisionSource source))
                return;
            source.Dispose();
            _visionSources.Remove(id);
        }

        private void OnValidate()
        {
            if (_useHostRadius)
                _radius = 0;
        }
    }
}